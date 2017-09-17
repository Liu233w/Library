using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Library.BookManage.Dto;
using Library.LibraryService.Dto;

namespace Library.LibraryService
{
    public interface ILibraryAppService : IApplicationService
    {
        Task<ListResultDto<BookDto>> SearchBook(SearchBookInput input);
    }
}