using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Library.Authorization;
using Library.BookManage.Dto;
using Library.LibraryService;
using Microsoft.EntityFrameworkCore;

namespace Library.BookManage
{
    [AbpAuthorize(PermissionNames.Pages_BookManage)]
    public class BookAppService : AsyncCrudAppService<Book, BookDto, long>, IBookAppService
    {
        private readonly IRepository<Copy, long> _copyRepository;
        private readonly BookInfoManager _bookInfoManager;

        public BookAppService(IRepository<Book, long> repository, IRepository<Copy, long> copyRepository, BookInfoManager bookInfoManager) : base(repository)
        {
            _copyRepository = copyRepository;
            _bookInfoManager = bookInfoManager;
        }

        public override async Task<BookDto> Get(EntityDto<long> input)
        {
            var book = await Repository.FirstOrDefaultAsync(input.Id);
            if (book == null)
            {
                throw new UserFriendlyException("Can't find the book");
            }
            return await MapBookAsync(book);
        }

        public async Task DeleteCopy(DeleteCopyInput input)
        {
            await EnsureCopyNotBorrowAsync(input.CopyId);
            await _copyRepository.DeleteAsync(input.CopyId);
        }

        private async Task EnsureCopyNotBorrowAsync(long copyId)
        {
            var record = await _bookInfoManager.FindRecordOrNullByCopyIdAsync(copyId);
            if (record != null)
            {
                throw new UserFriendlyException(
                    "That copy heavn't returned");
            }
        }

        public async Task<GetCopysOutput> GetCopys(GetCopysInput input)
        {
            var res = await _copyRepository.GetAll()
                .Where(item => item.BookId == input.BookId)
                .Select(item => item.Id)
                .ToListAsync();

            return new GetCopysOutput
            {
                Items = res
            };
        }

        public async Task<BookDto> GetBookByCopyId(GetBookByCopyIdInput input)
        {
            var copy = await _copyRepository.FirstOrDefaultAsync(input.CopyId);
            if (copy == null)
            {
                throw new UserFriendlyException("Can't find the copy");
            }

            await _bookInfoManager.LoadBookFromCopyAsync(copy);

            return await MapBookAsync(copy.Book);
        }

        public override async Task<PagedResultDto<BookDto>> GetAll(PagedAndSortedResultRequestDto input)
        {
            var books = await Repository.GetAllListAsync();
            var res = await books.MapAsync(MapBookAsync);
            return new PagedResultDto<BookDto>(res.Count, res);
        }

        public override async Task<BookDto> Create(BookDto input)
        {
            var book = MapToEntity(input);
            var bookId = await Repository.InsertAndGetIdAsync(book);

            await AddCopysAsync(bookId, input.Count);

            input.Id = bookId;
            return input;
        }

        private async Task AddCopysAsync(long bookId, int num)
        {
            for (int i = 0; i < num; ++i)
            {
                await _copyRepository.InsertAsync(new Copy
                {
                    BookId = bookId
                });
            }
        }

        public override async Task<BookDto> Update(BookDto input)
        {
            var copyCount = await _copyRepository.CountCopysByBookIdAsync(input.Id);
            if (input.Count < copyCount)
            {
                throw new UserFriendlyException(
                    "Can't decrease book count, " +
                    "please use Delete Copy to delete copys instead");
            }

            var addCount = input.Count - copyCount;
            await AddCopysAsync(input.Id, addCount);

            var book = ObjectMapper.Map<Book>(input);
            var res = await Repository.UpdateAsync(book);

            return ObjectMapper.Map<BookDto>(res);
        }

        private async Task<BookDto> MapBookAsync(Book book)
        {
            var res = MapToEntityDto(book);
            res.Count = await _copyRepository.CountCopysByBookIdAsync(book.Id);
            return res;
        }

        public override async Task Delete(EntityDto<long> input)
        {
            var book = await Repository.FirstOrDefaultAsync(input.Id);
            if (book == null)
            {
                throw new UserFriendlyException("Book isn't exist");
            }

            await _bookInfoManager.LoadCopysFromBookAsync(book);

            foreach (var copy in book.Copys)
            {
                await EnsureCopyNotBorrowAsync(copy.Id);
            }

            await base.Delete(input);
        }
    }
}