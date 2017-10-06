using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Library.Authorization;
using Library.BookManage.Dto;

namespace Library.BookManage
{
    [AbpAuthorize(PermissionNames.Pages_BookManage)]
    public class BookAppService : AsyncCrudAppService<Book, BookDto, long>, IBookAppService
    {
        public BookAppService(IRepository<Book, long> repository) : base(repository)
        {
        }
    }
}