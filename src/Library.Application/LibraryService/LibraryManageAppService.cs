using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public LibraryManageAppService(IRepository<Book, long> bookRepository, BookInfoManager bookInfoManager, IRepository<BorrowRecord, long> borrowedRecordRepository, INotificationPublisher notificationPublisher)
        {
            _bookRepository = bookRepository;
            _bookInfoManager = bookInfoManager;
            _borrowedRecordRepository = borrowedRecordRepository;
            _notificationPublisher = notificationPublisher;
        }

        public async Task<BookWithStatusAndRecord> GetBookStatus(GetBookStatusInput input)
        {
            var book = await _bookRepository.GetAsync(input.BookId);
            await _bookInfoManager.LoadAssociatedRecordsAsync(book);

            var statusBooks = ObjectMapper.Map<BookWithStatus>(book);
            statusBooks.Avaliable = book.Count - book.BorrowRecords.Count;

            var records = new List<BookUserState>();
            foreach (var record in book.BorrowRecords)
            {
                var user = await UserManager.GetUserByIdAsync(record.BorrowerUserId);
                records.Add(new BookUserState
                {
                    User = ObjectMapper.Map<UserDto>(user),
                    BorrowTimeLimit = record.CreationTime + LibraryConsts.UserMaxBorrowDuration,
                    Record = ObjectMapper.Map<BorrowRecordDto>(record)
                });
            }
            return new BookWithStatusAndRecord
            {
                Book = statusBooks,
                BorrowedBooks = records,
            };
        }

        public async Task BorrowBook(BorrowBookInput input)
        {
            await _bookInfoManager.EnsureBookExistAsync(input.BookId);

            if (await _bookInfoManager.GetAvailableAsync(input.BookId) <= 0)
            {
                throw new UserFriendlyException("That book has been borrowed out");
            }

            var user = await UserManager.FindByNameOrEmailAsync(input.UserNameOrEmail);

            var record = await _bookInfoManager.FindRecordOrNullAsync(input.BookId, user.Id);
            if (record != null)
            {
                throw new UserFriendlyException("The user has borrowed this book");
            }

            await _borrowedRecordRepository.InsertAsync(new BorrowRecord
            {
                BookId = input.BookId,
                BorrowerUserId = user.Id,
                RenewTime = 0
            });
        }

        public async Task ReturnBook(ReturnBookInput input)
        {
            await _bookInfoManager.EnsureBookExistAsync(input.BookId);

            var user = await UserManager.FindByNameOrEmailAsync(input.UserNameOrEmail);

            var record = await _bookInfoManager.FindRecordOrNullAsync(input.BookId, user.Id);
            if (record == null)
            {
                throw new UserFriendlyException("The user heavn't borrow this book");
            }

            await _borrowedRecordRepository.DeleteAsync(record);
        }

        public async Task<GetOutdatedBorrowRecordOutput> GetOutdatedBorrowRecord()
        {
            var records = await _borrowedRecordRepository.GetAll()
                .Where(item => item.GetOutdatedTime() >= DateTime.Now)
                .ToListAsync();

            return new GetOutdatedBorrowRecordOutput
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

        private async Task<BorrowRecordWithAdditionalInfo> GetOutputRecord(BorrowRecord record)
        {
            await _bookInfoManager.LoadBookFromRecordAsync(record);

            var res = ObjectMapper.Map<BorrowRecordWithAdditionalInfo>(record);

            var user = UserManager.GetUserByIdAsync(record.BorrowerUserId);
            res.UserInfo = ObjectMapper.Map<UserDto>(user);

            return res;
        }
    }
}