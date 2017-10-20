using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Notifications;
using Abp.UI;
using Library.Authorization;
using Library.Authorization.Users;
using Library.BookManage;
using Library.LibraryService.Dto;
using Library.Notification;
using Microsoft.EntityFrameworkCore;

namespace Library.LibraryService
{
    public class LibraryAppService : LibraryAppServiceBase, ILibraryAppService
    {
        private readonly IRepository<Book, long> _bookRepository;
        private readonly IRepository<BorrowRecord, long> _borrowRecordRepository;
        private readonly BookInfoManager _bookInfoManager;
        private readonly IUserNotificationManager _userNotificationManager;
        private readonly IRepository<Copy, long> _copyRepository;
        private readonly ICurrentUnitOfWorkProvider _currentUowProvider;
        private readonly IRepository<UserPhoto, long> _userPhotoRepository;

        public LibraryAppService(IRepository<Book, long> bookRepository, IRepository<BorrowRecord, long> borrowRecordRepository, BookInfoManager bookInfoManager, IUserNotificationManager userNotificationManager, IRepository<Copy, long> copyRepository, ICurrentUnitOfWorkProvider currentUowProvider, IRepository<UserPhoto, long> userPhotoRepository)
        {
            _bookRepository = bookRepository;
            _borrowRecordRepository = borrowRecordRepository;
            _bookInfoManager = bookInfoManager;
            _userNotificationManager = userNotificationManager;
            _copyRepository = copyRepository;
            _currentUowProvider = currentUowProvider;
            _userPhotoRepository = userPhotoRepository;
        }

        public async Task<BookWithStatusAndMine> GetBook(GetBookInput input)
        {
            var book = await _bookRepository.FirstOrDefaultAsync(input.BookId);
            if (book == null)
                throw new UserFriendlyException($"There is no book with id = {input.BookId}");

            if (AbpSession.UserId.HasValue)
            {
                return await GetBookWithUserStatusItemAsync(book, AbpSession.UserId.Value);
            }
            else
            {
                return await GetBookWithNoUserStatus(book);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Library)]
        public async Task RenewBook(RenewBookInput input)
        {
            var record = await _bookInfoManager.FindRecordOrNullByBookIdAsync(input.BookId, AbpSession.UserId.Value);
            if (record == null)
            {
                throw new UserFriendlyException("User haven't borrow that book or book is not exist");
            }

            if (record.RenewTime >= LibraryConsts.MaxRenewTime)
            {
                throw new UserFriendlyException($"You can only renew this book {LibraryConsts.MaxRenewTime} times");
            }

            ++record.RenewTime;
        }

        [AbpAuthorize(PermissionNames.Pages_Library)]
        public async Task<ListResultDto<BookWithStatusAndMine>> GetUserBook()
        {
            var borrowedList = await _borrowRecordRepository.GetAll()
                .Where(item => item.BorrowerUserId == AbpSession.UserId.Value)
                .Include(item => item.Copy)
                .ThenInclude(item => item.Book)
                .ToListAsync();

            var resList = new List<BookWithStatusAndMine>();
            foreach (var borrowRecord in borrowedList)
            {
                var item = ObjectMapper.Map<BookWithStatusAndMine>(borrowRecord.Copy.Book);
                item.BorrowTimeLimit = borrowRecord.GetOutdatedTime();
                item.Borrowed = true;
                item.Avaliable = await _bookInfoManager.GetAvailableAsync(borrowRecord.Copy.Book);
                item.Count = await _copyRepository.CountCopysByBookIdAsync(item.Id);

                resList.Add(item);
            }

            return new ListResultDto<BookWithStatusAndMine>(resList);
        }

        public async Task<ListResultDto<UserBorrowRecord>> GetBorrowRecords()
        {
            _currentUowProvider.Current.DisableFilter(AbpDataFilters.SoftDelete);
            var borrowedList = await _borrowRecordRepository.GetAll()
                .Where(item => item.BorrowerUserId == AbpSession.UserId.Value)
                .ToListAsync();

            var resList = new List<UserBorrowRecord>();
            foreach (var borrowRecord in borrowedList)
            {
                // 看起来禁用 filter 之后没法直接关联entity了，必须显示查询。否则单元测试会报错。
                var copy = await _copyRepository.GetAsync(borrowRecord.CopyId);
                var book = await _bookRepository.GetAsync(copy.BookId);
                var item = ObjectMapper.Map<UserBorrowRecord>(book);
                item.Returned = borrowRecord.IsDeleted;
                if (item.Returned)
                {
                    item.Time = borrowRecord.DeletionTime.Value;
                }
                else
                {
                    item.Time = borrowRecord.GetOutdatedTime();
                }
                item.Avaliable = await _bookInfoManager.GetAvailableAsync(book);
                item.Count = await _copyRepository.CountCopysByBookIdAsync(item.Id);

                resList.Add(item);
            }

            return new ListResultDto<UserBorrowRecord>(resList);
        }

        [AbpAuthorize]
        public async Task<ListResultDto<BroadcastUserNotificationDto>> GetMyNotifications(GetMyNotificationsInput input)
        {
            var notifications = await _userNotificationManager.GetUserNotificationsAsync(
                new UserIdentifier(AbpSession.TenantId, AbpSession.UserId.Value), 
                input.NotificationState);

            var res = notifications.Map(item =>
            {
                var noti = item.Notification;
                var notiData = noti.Data as BroadcastNotificationData;
                return new BroadcastUserNotificationDto()
                {
                    UserNotificationId = item.Id,
                    State = item.State,
                    Content = notiData.Content,
                    Publisher = notiData.Publisher,
                    PublishTime = noti.CreationTime
                };
            });

            return new ListResultDto<BroadcastUserNotificationDto>(res);
        }

        public async Task MarkNotificationAsRead(MarkNotificationAsReadInput input)
        {
            var noti = await _userNotificationManager.GetUserNotificationAsync(
                AbpSession.TenantId, input.UserNotificationId);
            if (noti.Notification.NotificationName != NotificationType.BroadcastNotification)
            {
                throw new UserFriendlyException("This Notification is not exist",
                    new Exception("User try to mark a none-broadcastNotification as readed"));
            }

            await _userNotificationManager.UpdateUserNotificationStateAsync(AbpSession.TenantId,
                input.UserNotificationId, UserNotificationState.Read);
        }

        [AbpAuthorize]
        public async Task<GetNotificationCountOutput> GetNotificationCount()
        {
            var userIdentifier=new UserIdentifier(AbpSession.TenantId,AbpSession.UserId.Value);

            return new GetNotificationCountOutput()
            {
                ReadedCount = await _userNotificationManager.GetUserNotificationCountAsync(
                    userIdentifier, UserNotificationState.Read),
                UnreadCount = await _userNotificationManager.GetUserNotificationCountAsync(
                    userIdentifier, UserNotificationState.Unread)
            };
        }

        [AbpAuthorize]
        public async Task<GetUserPhotoOutput> GetUserPhoto(GetUserPhotoInput input)
        {
            long userId;
            if (input.UserId.HasValue)
            {
                if (!await IsGrantedAsync(PermissionNames.Pages_LibraryManage))
                {
                    throw new AbpAuthorizationException("You can't check other user's photo");
                }

                userId = input.UserId.Value;
            }
            else
            {
                userId = AbpSession.UserId.Value;
            }

            var photo = await _userPhotoRepository.FirstOrDefaultAsync(
                item => item.UserId == userId);
            return new GetUserPhotoOutput
            {
                PhotoId = photo?.PhotoId
            };
        }

        [AbpAuthorize]
        public async Task SetUserPhoto(SetUserPhotoInput input)
        {
            var photo = await _userPhotoRepository.FirstOrDefaultAsync(
                item => item.UserId == AbpSession.UserId.Value);
            if (photo == null)
            {
                await _userPhotoRepository.InsertAsync(new UserPhoto
                {
                    UserId = AbpSession.UserId.Value,
                    PhotoId = input.PhotoId
                });
            }
            else
            {
                photo.PhotoId = input.PhotoId;
            }
        }

        public async Task<ListResultDto<BookWithStatusAndMine>> GetBookList()
        {
            var books = await _bookRepository.GetAllListAsync();

            // 未登录的用户不需要获取当前状态
            if (!AbpSession.UserId.HasValue)
            {
                return new ListResultDto<BookWithStatusAndMine>(
                    await books.MapAsync(GetBookWithNoUserStatus));
            }

            var userId = AbpSession.UserId.Value;

            return new ListResultDto<BookWithStatusAndMine>(
                await books.MapAsync(async item => 
                    await GetBookWithUserStatusItemAsync(item, userId)));
        }

        private async Task<BookWithStatus> GetBookWithStatusFromBookAsync(Book book)
        {
            var res = ObjectMapper.Map<BookWithStatus>(book);
            res.Avaliable = await _bookInfoManager.GetAvailableAsync(book);
            res.Count = await _copyRepository.CountCopysByBookIdAsync(book.Id);
            return res;
        }

        private async Task<BookWithStatusAndMine> GetBookWithNoUserStatus(Book book)
        {
            var t = await GetBookWithStatusFromBookAsync(book);
            // bool 的默认值是 false，因此默认就是没有借书
            return ObjectMapper.Map<BookWithStatusAndMine>(t);
        }

        private async Task<BookWithStatusAndMine> GetBookWithUserStatusItemAsync(Book book, long userId)
        {
            var bookWithStatus = await GetBookWithStatusFromBookAsync(book);
            var res = ObjectMapper.Map<BookWithStatusAndMine>(bookWithStatus);

            var record = await _bookInfoManager.FindRecordOrNullByBookIdAsync(res.Id, userId);
            if (record == null)
            {
                // 没有借这本书
                res.Borrowed = false;
            }
            else
            {
                res.Borrowed = true;
                res.BorrowTimeLimit = record.GetOutdatedTime();
            }

            return res;
        }
    }
}