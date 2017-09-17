using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Library.BookManage;
using Library.BookManage.Dto;
using Library.LibraryService.Dto;

namespace Library.LibraryService
{
    public class LibraryAppService : LibraryAppServiceBase, ILibraryAppService
    {
        private readonly IRepository<Book, long> _bookRepository;

        public LibraryAppService(IRepository<Book, long> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<ListResultDto<BookDto>> SearchBook(SearchBookInput input)
        {
            Func<Book, string> selector; 
            switch (input.Type)
            {
                case SearchBookType.Author:
                    selector = book => book.Author;
                    break;
                case SearchBookType.Isbn:
                    selector = book => book.Isbn;
                    break;
                case SearchBookType.Publish:
                    selector = book => book.Publish;
                    break;
                case SearchBookType.Title:
                    selector = book => book.Isbn;
                    break;
                default:
                    throw new UserFriendlyException("Type not supported");
            }

            var lst = _bookRepository.GetAllListAsync(
                book => selector(book).Contains(input.KeyWord));

            return new ListResultDto<BookDto>(ObjectMapper.Map<List<BookDto>>(lst));
        }
    }
}