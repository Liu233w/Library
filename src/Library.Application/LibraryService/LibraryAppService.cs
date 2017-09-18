using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Library.BookManage;
using Library.BookManage.Dto;
using Library.LibraryService.Dto;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;

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

        public async Task RenewBook(RenewBookInput input)
        {
            throw new NotImplementedException();
        }

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
                item.BorrowTimeLimit = borrowRecord.CreationTime + LibraryConsts.UserMaxBorrowDuration
                                       + borrowRecord.RenewTime * LibraryConsts.RenewDuration;
                item.Borrowed = true;
                item.Avaliable = await _bookInfoManager.GetAvailableAsync(borrowRecord.Book);

                resList.Add(item);
            }

            return new ListResultDto<BookWithStatusAndMine>(resList);
        }

        public async Task<ListResultDto<BookWithStatusAndMine>> GetBookListOutput()
        {
            throw new NotImplementedException();
        }

        public async Task BorrowBook(BorrowBookInput input)
        {
            throw new NotImplementedException();
        }
    }
}