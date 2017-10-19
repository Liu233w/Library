using System.Threading.Tasks;
using Abp.Application.Services;
using Library.BookManage.Dto;

namespace Library.BookManage
{
    public interface IBookAppService : IAsyncCrudAppService<BookDto, long>
    {
        Task DeleteCopy(DeleteCopyInput input);

        /// <summary>
        /// 获取一本书的所有副本
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetCopysOutput> GetCopys(GetCopysInput input);

        /// <summary>
        /// 根据CopyId来获得书籍信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<BookDto> GetBookByCopyId(GetBookByCopyIdInput input);
    }
}