using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.UI;
using Library.BookManage;
using Microsoft.EntityFrameworkCore;

namespace Library.LibraryService
{
    public class BookInfoManager : ITransientDependency
    {
        private readonly IRepository<Book, long> _bookRepository;
        private readonly IRepository<BorrowRecord, long> _borrowRecordRepository;

        public BookInfoManager(IRepository<Book, long> bookRepository, IRepository<BorrowRecord, long> borrowRecordRepository)
        {
            _bookRepository = bookRepository;
            _borrowRecordRepository = borrowRecordRepository;
        }

        [UnitOfWork]
        public async Task LoadAssociatedRecordsAsync(Book book)
        {
            await _bookRepository.GetDbContext()
                .Entry(book)
                .Collection(item => item.BorrowRecords)
                .LoadAsync();
        }

        [UnitOfWork]
        public async Task LoadBookFromRecordAsync(BorrowRecord record)
        {
            await _borrowRecordRepository.GetDbContext()
                .Entry(record)
                .Reference(item => item.Book)
                .LoadAsync();
        }

        /// <summary>
        /// 获取这本书还剩下几本可以借出去
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<int> GetAvailableAsync(Book book)
        {
            var borrowCount = await _borrowRecordRepository.GetAll()
                .Where(item => item.BookId == book.Id)
                .CountAsync();
            return book.Count - borrowCount;
        }

        [UnitOfWork]
        public async Task<int> GetAvailableAsync(long bookId)
        {
            var book = await _bookRepository.GetAsync(bookId);
            return await GetAvailableAsync(book);
        }

        /// <summary>
        /// 查找 Book，如果不存在，抛出UserFriendlyException；否则返回 Book
        /// </summary>
        /// <exception cref="UserFriendlyException"></exception>
        [UnitOfWork]
        public async Task<Book> EnsureBookExistAsync(long bookId)
        {
            var book = await _bookRepository.FirstOrDefaultAsync(bookId);
            if (book == null)
            {
                throw new UserFriendlyException($"There is no book which id = {bookId}");
            }
            return book;
        }

        /// <summary>
        /// 获取某用户对某本书的借阅情况，如果借了；返回借阅记录，如果没借（或者还了），返回null
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public Task<BorrowRecord> FindRecordOrNullAsync(long bookId, long userId)
        {
            return _borrowRecordRepository.FirstOrDefaultAsync(
                item => item.BookId == bookId && item.BorrowerUserId == userId);
        }
    }
}