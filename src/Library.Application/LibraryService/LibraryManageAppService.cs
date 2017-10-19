using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Notifications;
using Abp.UI;
using Library.Authorization;
using Library.Authorization.Users;
using Library.BookManage;
using Library.LibraryService.Dto;
using Library.Notification;
using Library.Users.Dto;
using Microsoft.EntityFrameworkCore;

namespace Library.LibraryService
{
    [AbpAuthorize(PermissionNames.Pages_LibraryManage)]
    public class LibraryManageAppService : LibraryAppServiceBase, ILibraryManageAppService
    {
        private readonly IRepository<Book, long> _bookRepository;
        private readonly IRepository<BorrowRecord, long> _borrowedRecordRepository;
        private readonly BookInfoManager _bookInfoManager;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IRepository<TenantNotificationInfo, Guid> _tenantNotificationRepository;
        private readonly IRepository<UserNotificationInfo, Guid> _userNotificationRepository;

        public LibraryManageAppService(IRepository<Book, long> bookRepository, BookInfoManager bookInfoManager, IRepository<BorrowRecord, long> borrowedRecordRepository, INotificationPublisher notificationPublisher, IRepository<TenantNotificationInfo, Guid> tenantNotificationRepository, IRepository<UserNotificationInfo, Guid> userNotificationRepository)
        {
            _bookRepository = bookRepository;
            _bookInfoManager = bookInfoManager;
            _borrowedRecordRepository = borrowedRecordRepository;
            _notificationPublisher = notificationPublisher;
            _tenantNotificationRepository = tenantNotificationRepository;
            _userNotificationRepository = userNotificationRepository;
        }

        public async Task<BookWithStatusAndRecord> GetBookStatus(GetBookStatusInput input)
        {
            var book = await _bookRepository.GetAsync(input.BookId);
            await _bookInfoManager.LoadAssociatedRecordsAsync(book);

            var statusBooks = ObjectMapper.Map<BookWithStatus>(book);
            statusBooks.Avaliable = await _bookInfoManager.GetAvailableAsync(book);

            var records = new List<CopyUserState>();
            foreach (var copy in book.Copys)
            {
                var record = copy.BorrowRecord;
                if (record == null)
                {
                    continue;
                }

                var user = await UserManager.GetUserByIdAsync(record.BorrowerUserId);
                records.Add(new CopyUserState
                {
                    User = ObjectMapper.Map<UserDto>(user),
                    BorrowTimeLimit = record.CreationTime + LibraryConsts.UserMaxBorrowDuration,
                    Record = ObjectMapper.Map<BorrowRecordDto>(record)
                });
            }
            return new BookWithStatusAndRecord
            {
                Book = statusBooks,
                BorrowedCopysAndRecord = records,
            };
        }

        public async Task BorrowBook(BorrowBookInput input)
        {
            var copy = await _bookInfoManager.EnsureCopyExistAsync(input.CopyId);

            if (copy.BorrowRecordId.HasValue)
            {
                throw new UserFriendlyException("That copy has been borrowed out");
            }

            var user = await UserManager.FindByNameOrEmailAsync(input.UserNameOrEmail);

            var record = await _bookInfoManager.FindRecordOrNullByBookIdAsync(copy.BookId, user.Id);
            if (record != null)
            {
                throw new UserFriendlyException("The user has borrowed this book");
            }

            var recordId = await _borrowedRecordRepository.InsertAndGetIdAsync(new BorrowRecord
            {
                CopyId = input.CopyId,
                BorrowerUserId = user.Id,
                RenewTime = 0
            });
            copy.BorrowRecordId = recordId;
        }

        public async Task ReturnBook(ReturnBookInput input)
        {
            var copy = await _bookInfoManager.EnsureCopyExistAsync(input.CopyId);

            var record = await _borrowedRecordRepository.FirstOrDefaultAsync(input.CopyId);
            if (record == null)
            {
                throw new UserFriendlyException("The copy heavn't been borrowed yet");
            }

            await _borrowedRecordRepository.DeleteAsync(record);
            copy.BorrowRecordId = null;
            copy.BorrowRecord = null;
        }

        public async Task<GetOutdatedBorrowRecordOutput> GetOutdatedBorrowRecord()
        {
            var records = await _borrowedRecordRepository.GetAll()
                .Where(item => item.GetOutdatedTime() <= DateTime.Now)
                .ToListAsync();

            return new GetOutdatedBorrowRecordOutput
            {
                Items = await records.MapAsync(GetOutputRecord)
            };
        }

        public async Task<GetUnreturnedRecordOutput> GetUnreturnedRecord()
        {
            var records = await _borrowedRecordRepository.GetAll()
                .ToListAsync();

            return new GetUnreturnedRecordOutput
            {
                Items = await records.MapAsync(GetOutputRecord)
            };
        }

        public async Task PublishNotification(PublishNotificationInput input)
        {
            var user = await GetCurrentUserAsync();

            await _notificationPublisher.PublishAsync(NotificationType.BroadcastNotification,
                new BroadcastNotificationData(input.Content, user.FullName));
        }

        public async Task<ListResultDto<BroadcastNotificationDto>> GetNotificationList()
        {
            var res = await _tenantNotificationRepository.GetAll()
                .Where(item => item.NotificationName == NotificationType.BroadcastNotification)
                .ToListAsync();

            return new ListResultDto<BroadcastNotificationDto>(
                res.Map(item =>
                {
                    var notification =item.ToTenantNotification();
                    var data = notification.Data as BroadcastNotificationData;
                    return new BroadcastNotificationDto()
                    {
                        Content = data.Content,
                        Publisher = data.Publisher,
                        PublishTime = notification.CreationTime,
                        NotificationId = notification.Id
                    };
                }));
        }

        public async Task DeleteNotification(DeleteNotificationInput input)
        {
            var notification = await _tenantNotificationRepository.GetAsync(input.NotificationId);

            if (notification.NotificationName != NotificationType.BroadcastNotification)
            {
                throw new UserFriendlyException("Notification is not exist", 
                    new Exception("Try to delete a notification that is not a BroadcastNotification"));
            }

            await _userNotificationRepository.DeleteAsync(
                item => item.TenantNotificationId == notification.Id);

            await _tenantNotificationRepository.DeleteAsync(notification);
        }

        private async Task<BorrowRecordWithAdditionalInfo> GetOutputRecord(BorrowRecord record)
        {
            await _bookInfoManager.LoadBookFromRecordAsync(record);

            var res = ObjectMapper.Map<BorrowRecordWithAdditionalInfo>(record);

            var user = await UserManager.GetUserByIdAsync(record.BorrowerUserId);
            res.UserInfo = ObjectMapper.Map<UserDto>(user);

            return res;
        }
    }
}