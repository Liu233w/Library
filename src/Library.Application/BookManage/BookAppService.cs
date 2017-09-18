﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Library.BookManage.Dto;

namespace Library.BookManage
{
    public class BookAppService : AsyncCrudAppService<Book, BookDto, long>, IBookAppService
    {
        public BookAppService(IRepository<Book, long> repository) : base(repository)
        {
        }

        public async Task<BookStatusWithUserList> GetBookStatus(GetBookStatusInput input)
        {
            throw new System.NotImplementedException();
        }
    }
}