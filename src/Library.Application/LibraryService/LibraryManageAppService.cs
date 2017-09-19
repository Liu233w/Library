using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Library.Authorization.Users;
using Library.BookManage;
using Library.LibraryService.Dto;
using Library.Users.Dto;
using Microsoft.EntityFrameworkCore;

namespace Library.LibraryService
{
    public class LibraryManageAppService : LibraryAppServiceBase, ILibraryManageAppService
    {
        private readonly IRepository<Book, long> _bookRepository;
        private readonly IRepository<BorrowRecord, long> _borrowedRecordRepository;
        private readonly BookInfoManager _bookInfoManager;
        private readonly UserManager _userManager;

        public LibraryManageAppService(IRepository<Book, long> bookRepository, BookInfoManager bookInfoManager, UserManager userManager, IRepository<BorrowRecord, long> borrowedRecordRepository)
        {
            _bookRepository = bookRepository;
            _bookInfoManager = bookInfoManager;
            _userManager = userManager;
            _borrowedRecordRepository = borrowedRecordRepository;
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
                var user = await _userManager.FindByIdAsync(record.BorrowerUserId.ToString());
                records.Add(new BookUserState
                {
                    User = user.MapTo<UserDto>(),
                    BorrowTimeLimit = record.CreationTime + LibraryConsts.UserMaxBorrowDuration,
                    Record = record.MapTo<BorrowRecordDto>()
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

            var user = await _userManager.FindByNameOrEmailAsync(input.UserNameOrEmail);

            var record = await _bookInfoManager.FindRecordOrNull(input.BookId, user.Id);
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

            var user = await _userManager.FindByNameOrEmailAsync(input.UserNameOrEmail);

            var record = await _bookInfoManager.FindRecordOrNull(input.BookId, user.Id);
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

        private async Task<BorrowRecordWithBookTitleAndOutdatedTime> GetOutputRecord(BorrowRecord record)
        {
            await _bookInfoManager.LoadBookFromRecordAsync(record);

            var res = record.MapTo<BorrowRecordWithBookTitleAndOutdatedTime>();

            res.BookTitle = record.Book.Title;
            res.BorrowTimeLimit = record.GetOutdatedTime();

            return res;
        }
    }
}