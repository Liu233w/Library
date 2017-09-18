using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Library.BookManage.Dto;

namespace Library.BookManage
{
    public interface IBookAppService : IAsyncCrudAppService<BookDto, long>
    {
        /// <summary>
        /// 获取某本书的情况，包括借阅了这本书的用户、应还时间
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<BookStatusWithUserList> GetBookStatus(GetBookStatusInput input);
    }
}