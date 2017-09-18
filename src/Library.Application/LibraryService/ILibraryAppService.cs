using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Library.BookManage.Dto;
using Library.LibraryService.Dto;

namespace Library.LibraryService
{
    public interface ILibraryAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户借书情况
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<BookWithStatusAndMine>> GetUserBook();

        /// <summary>
        /// 获取书籍列表（给用户看的）
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<BookWithStatusAndMine>> GetBookListOutput();

        Task BorrowBook(BorrowBookInput input);
    }
}