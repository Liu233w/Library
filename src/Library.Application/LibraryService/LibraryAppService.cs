using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Library.BookManage;
using Library.LibraryService.Dto;
using Microsoft.EntityFrameworkCore;

namespace Library.LibraryService
{
    public class LibraryAppService : LibraryAppServiceBase, ILibraryAppService
    {
        private readonly IRepository<Book, long> _bookRepository;
        private readonly IRepository<BorrowRecord, long> _borrowRecordRepository;
        private readonly BookInfoManager _bookInfoManager;

        public LibraryAppService(IRepository<Book, long> bookRepository, IRepository<BorrowRecord, long> borrowRecordRepository, BookInfoManager bookInfoManager)
        {
            _bookRepository = bookRepository;
            _borrowRecordRepository = borrowRecordRepository;
            _bookInfoManager = bookInfoManager;
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

        [AbpAuthorize]
        public async Task RenewBook(RenewBookInput input)
        {
            var record = await _bookInfoManager.GetUserRecordOrNullAsync(AbpSession.UserId.Value, input.BookId);
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

        [AbpAuthorize]
        public async Task<ListResultDto<BookWithStatusAndMine>> GetUserBook()
        {
            var borrowedList = await _borrowRecordRepository.GetAll()
                .Where(item => item.BorrowerUserId == AbpSession.UserId.Value)
                .Include(item => item.Book)
                .ToListAsync();

            var resList = new List<BookWithStatusAndMine>();
            foreach (var borrowRecord in borrowedList)
            {
                var item = ObjectMapper.Map<BookWithStatusAndMine>(borrowRecord.Book);
                item.BorrowTimeLimit = borrowRecord.GetOutdatedTime();
                item.Borrowed = true;
                item.Avaliable = await _bookInfoManager.GetAvailableAsync(borrowRecord.Book);

                resList.Add(item);
            }

            return new ListResultDto<BookWithStatusAndMine>(resList);
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
            var res = book.MapTo<BookWithStatus>();
            res.Avaliable = await _bookInfoManager.GetAvailableAsync(book);
            return res;
        }

        private async Task<BookWithStatusAndMine> GetBookWithNoUserStatus(Book book)
        {
            var t = await GetBookWithStatusFromBookAsync(book);
            // bool 的默认值是 false，因此默认就是没有借书
            return t.MapTo<BookWithStatusAndMine>();
        }

        private async Task<BookWithStatusAndMine> GetBookWithUserStatusItemAsync(Book book, long userId)
        {
            var bookWithStatus = await GetBookWithStatusFromBookAsync(book);
            var res = bookWithStatus.MapTo<BookWithStatusAndMine>();

            var record = await _bookInfoManager.GetUserRecordOrNullAsync(userId, res.Id);
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