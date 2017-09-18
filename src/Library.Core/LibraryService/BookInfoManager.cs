using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
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

        public async Task LoadAssociatedRecordAsync(Book book)
        {
            await _bookRepository.GetDbContext()
                .Entry(book)
                .Collection(item => item.BorrowRecords)
                .LoadAsync();
        }

        /// <summary>
        /// 获取这本书还剩下几本可以借出去
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public async Task<int> GetAvailableAsync(Book book)
        {
            var borrowCount = await _borrowRecordRepository.GetAll()
                .Where(item => item.BookId == book.Id)
                .CountAsync();
            return book.Count - borrowCount;
        }

        public async Task<int> GetAvailableAsync(long bookId)
        {
            var book = await _bookRepository.GetAsync(bookId);
            return await GetAvailableAsync(book);
        }

        /// <summary>
        /// 获取某用户对某本书的借阅情况，如果借了；返回借阅记录，如果没借（或者还了），返回null
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public async Task<BorrowRecord> GetUserRecordAsync(long userId, long bookId)
        {
            return await _borrowRecordRepository.FirstOrDefaultAsync(
                item => item.CreatorUserId == userId && item.BookId == bookId);
        }

        /// <summary>
        /// 查找 Book，如果不存在，抛出UserFriendlyException；否则返回 Book
        /// </summary>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<Book> EnsureBookExistAsync(long bookId)
        {
            var book = await _bookRepository.FirstOrDefaultAsync(bookId);
            if (book == null)
            {
                throw new UserFriendlyException($"There is no book which id = {bookId}");
            }
            return book;
        }

        public Task<BorrowRecord> FindRecordOrNull(long bookId, long userId)
        {
            return _borrowRecordRepository.FirstOrDefaultAsync(
                item => item.BookId == bookId && item.BorrowerUserId == userId);
        }
    }
}