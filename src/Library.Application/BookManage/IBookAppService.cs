using System.Threading.Tasks;
using Abp.Application.Services;
using Library.BookManage.Dto;

namespace Library.BookManage
{
    public interface IBookAppService : IAsyncCrudAppService<BookDto, long>
    {
        Task DeleteCopy(DeleteCopyInput input);
    }
}