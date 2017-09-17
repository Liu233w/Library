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
            List<Book> lst;
            switch (input.Type)
            {
                case SearchBookType.Author:
                    lst = await _bookRepository.GetAllListAsync(
                        book => book.Author.Contains(input.KeyWord));
                    break;
                case SearchBookType.Isbn:
                    lst = await _bookRepository.GetAllListAsync(
                        book => book.Isbn.Contains(input.KeyWord));
                    break;
                case SearchBookType.Publish:
                    lst = await _bookRepository.GetAllListAsync(
                        book => book.Publish.Contains(input.KeyWord));
                    break;
                case SearchBookType.Title:
                    lst = await _bookRepository.GetAllListAsync(
                        book => book.Title.Contains(input.KeyWord));
                    break;
                default:
                    throw new UserFriendlyException("Type not supported");
            }

            return new ListResultDto<BookDto>(ObjectMapper.Map<List<BookDto>>(lst));
        }
    }
}