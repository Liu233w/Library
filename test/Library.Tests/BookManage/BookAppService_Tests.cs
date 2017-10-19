using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.ObjectMapping;
using Abp.UI;
using Library.BookManage;
using Library.BookManage.Dto;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Library.Tests.BookManage
{
    public class BookAppService_Tests : LibraryTestBase
    {
        private readonly IBookAppService _bookAppService;

        public BookAppService_Tests()
        {
            _bookAppService = Resolve<BookAppService>();
        }

        private BookDto Map(Book book)
        {
            return new BookDto
            {
                Isbn = book.Isbn,
                Count = 0,
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Publish = book.Publish,
                Author = book.Author,
                Location = book.Location
            };
        }

        [Fact]
        public async Task Get_ShouldReturnCorrectly()
        {
            await InjectBooksDataAsync();

            //Act
            var res = await _bookAppService.Get(new EntityDto<long>(Book1.Id));

            //Asserts
            res.Isbn.ShouldBe(Book1.Isbn);
            res.Count.ShouldBe(2);
        }

        [Fact]
        public async Task DeleteCopy_ShouldActCorrectly()
        {
            await InjectBooksDataAsync();
            await InjectTestUser();
            var record = await InjectBorrowRecord1AndGetAsync(0);

            //Act
            await _bookAppService.DeleteCopy(new DeleteCopyInput
            {
                CopyId = Book1Copy1.Id
            });

            //Assert
            await UsingDbContextAsync(async ctx =>
            {
                var copy = await ctx.Copys.FirstOrDefaultAsync(item => item.Id == Book1Copy1.Id);
                copy.ShouldBe(null, "The copy should be deleted");

                var newRecord = await ctx.BorrowRecords.FirstOrDefaultAsync(item => item.Id == record.Id);
                newRecord.ShouldNotBeNull("The record shouldn't be deleted");
                newRecord.CopyId.ShouldBe(Book1Copy1.Id);
            });
        }

        [Fact]
        public async Task GetAll_ShouldReturnCorrectly()
        {
            await InjectBooksDataAsync();

            //Act
            var res = await _bookAppService.GetAll(new PagedAndSortedResultRequestDto());

            //Asserts
            res.TotalCount.ShouldBe(2);
            res.Items.First()
                .Count.ShouldBe(2);
        }

        [Fact]
        public async Task Create_ShouldActCorrectly()
        {
            //Act
            var res = await _bookAppService.Create(new BookDto
            {
                Isbn = "1234567890",
                Author = "qwe",
                Count = 3,
                Description = "desc",
                Location = "a1",
                Publish = "pub1",
                Title = "Ruby"
            });

            //Asserts
            res.Count.ShouldBe(3);
            res.Title.ShouldBe("Ruby");
            res.Id.ShouldNotBe(0);

            await UsingDbContextAsync(async ctx =>
            {
                var book = await ctx.Books.FirstOrDefaultAsync(item => item.Title == "Ruby");
                book.ShouldNotBeNull();
                book.Location.ShouldBe("a1");

                var copyCount = await ctx.Copys.CountAsync(
                    item => item.BookId == book.Id);
                copyCount.ShouldBe(3);
            });
        }

        [Fact]
        public async Task Update_ShouldActCorrectlyWhenNotModifyCount()
        {
            await InjectBooksDataAsync();

            var dto = Map(Book1);
            dto.Count = 2;
            dto.Title = "Perl";

            //Act
            await _bookAppService.Update(dto);

            //Asserts
            await UsingDbContextAsync(async ctx =>
            {
                var book = await ctx.Books.FindAsync(dto.Id);
                book.Title.ShouldBe("Perl");

                var copyCount = await ctx.Copys.CountAsync(
                    item => item.BookId == book.Id);
                copyCount.ShouldBe(2);
            });
        }

        [Fact]
        public async Task Update_ShouldAddCopyWhenIncreaseCount()
        {
            var dto = Map(Book1);
            dto.Count = 2;
            dto = await _bookAppService.Create(dto);
            dto.Count = 4;

            //Act
            await _bookAppService.Update(dto);

            //Asserts
            await UsingDbContextAsync(async ctx =>
            {
                var copyCount = await ctx.Copys.CountAsync(
                    item => item.BookId == dto.Id);
                copyCount.ShouldBe(4);
            });
        }

        [Fact]
        public async Task Update_ShouldThrowWhenDecreaseCount()
        {
            var dto = Map(Book1);
            dto.Count = 2;
            dto = await _bookAppService.Create(dto);
            dto.Count = 1;

            //Act
            var task = _bookAppService.Update(dto);

            //Asserts
            await task.ShouldThrowAsync<UserFriendlyException>();
        }

        [Fact]
        public async Task GetCopys_ShouldReturnCorrectly()
        {
            await InjectBooksDataAsync();

            //Act
            var res = await _bookAppService.GetCopys(new GetCopysInput
            {
                BookId = Book1.Id
            });

            //Asserts
            var items = res.Items.ToList();
            items.Count.ShouldBe(2);
            items.ShouldContain(Book1Copy1.Id);
            items.ShouldContain(Book1Copy2.Id);
        }

        [Fact]
        public async Task GetBookByCopyId_ShouldReturnCorrectly()
        {
            await InjectBooksDataAsync();

            //Act
            var res = await _bookAppService.GetBookByCopyId(new GetBookByCopyIdInput
            {
                CopyId = Book1Copy1.Id
            });

            //Asserts
            res.Count.ShouldBe(2);
            res.Title.ShouldBe(Book1.Title);
        }
    }
}