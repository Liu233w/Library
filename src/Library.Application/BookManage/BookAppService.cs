using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Library.BookManage.Dto;
using Library.LibraryService;

namespace Library.BookManage
{
    public class BookAppService : AsyncCrudAppService<Book, BookDto, long>, IBookAppService
    {
        private readonly IRepository<BorrowRecord, long> _borrowRecordRepository;

        public BookAppService(IRepository<Book, long> repository, IRepository<BorrowRecord, long> borrowRecordRepository) : base(repository)
        {
            _borrowRecordRepository = borrowRecordRepository;
        }

        public async Task<BookStatusWithUserList> GetBookStatus(GetBookStatusInput input)
        {
            throw new System.NotImplementedException();
        }
    }
}