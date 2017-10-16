using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.UI;
using JetBrains.Annotations;
using Library.BookManage;
using Microsoft.EntityFrameworkCore;

namespace Library.LibraryService
{
    public class BookInfoManager : ITransientDependency
    {
        private readonly IRepository<Book, long> _bookRepository;
        private readonly IRepository<BorrowRecord, long> _borrowRecordRepository;
        private readonly IRepository<Copy, long> _copyRepository;

        public BookInfoManager(IRepository<Book, long> bookRepository, IRepository<BorrowRecord, long> borrowRecordRepository, IRepository<Copy, long> copyRepository)
        {
            _bookRepository = bookRepository;
            _borrowRecordRepository = borrowRecordRepository;
            _copyRepository = copyRepository;
        }

        [UnitOfWork]
        public async Task LoadAssociatedRecordsAsync(Copy copy)
        {
            await _copyRepository.GetDbContext()
                .Entry(copy)
                .Reference(item => item.BorrowRecord)
                .LoadAsync();
        }

        [UnitOfWork]
        public async Task LoadAssociatedRecordsAsync(Book book)
        {
            await _bookRepository.GetDbContext()
                .Entry(book)
                .Collection(item => item.Copys)
                .LoadAsync();

            foreach (var copy in book.Copys)
            {
                await _copyRepository.GetDbContext()
                    .Entry(copy)
                    .Reference(item => item.BorrowRecord)
                    .LoadAsync();
            }
        }

        [UnitOfWork]
        public async Task LoadBookFromRecordAsync(BorrowRecord record)
        {
            await _borrowRecordRepository.GetDbContext()
                .Entry(record)
                .Reference(item => item.Copy)
                .LoadAsync();

            await LoadBookFromCopyAsync(record.Copy);
        }

        [UnitOfWork]
        public async Task LoadBookFromCopyAsync(Copy copy)
        {
            await _copyRepository.GetDbContext()
                .Entry(copy)
                .Reference(item => item.Book)
                .LoadAsync();
        }

        public async Task LoadCopysFromBookAsync(Book book)
        {
            await _bookRepository.GetDbContext()
                .Entry(book)
                .Collection(item => item.Copys)
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
            var availableCount = await _copyRepository.GetAll()
                .Where(item => item.BookId == book.Id && !item.BorrowRecordId.HasValue)
                .CountAsync();
            return availableCount;
        }

        [UnitOfWork]
        public async Task<int> GetAvailableAsync(long bookId)
        {
            var book = await _bookRepository.GetAsync(bookId);
            return await GetAvailableAsync(book);
        }

        /// <summary>
        /// 查找 Copy，如果不存在，抛出UserFriendlyException；否则返回 Copy
        /// </summary>
        /// <exception cref="UserFriendlyException"></exception>
        [UnitOfWork]
        public async Task<Copy> EnsureCopyExistAsync(long copyId)
        {
            var copy = await _copyRepository.FirstOrDefaultAsync(copyId);
            if (copy == null)
            {
                throw new UserFriendlyException($"There is no copy which id = {copyId}");
            }
            return copy;
        }

        /// <summary>
        /// 获取某用户对某本书的借阅情况，如果借了；返回借阅记录，如果没借（或者还了），返回null
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public Task<BorrowRecord> FindRecordOrNullByBookIdAsync(long bookId, long userId)
        {
            return _borrowRecordRepository.GetAll()
                .Where(item => item.BorrowerUserId == userId)
                .Include(item => item.Copy).ThenInclude(item => item.Book)
                .Where(item => item.Copy.BookId == bookId)
                .FirstOrDefaultAsync();
        }
    }
}