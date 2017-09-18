using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Library.BookManage;
using Library.BookManage.Dto;
using Library.LibraryService.Dto;
using Remotion.Linq.Clauses;

namespace Library.LibraryService
{
    public class LibraryAppService : LibraryAppServiceBase, ILibraryAppService
    {
        private readonly IRepository<Book, long> _bookRepository;

        public LibraryAppService(IRepository<Book, long> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<ListResultDto<BookWithStatusAndMine>> GetUserBook()
        {
            throw new NotImplementedException();
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