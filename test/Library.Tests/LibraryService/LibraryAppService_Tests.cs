using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel;
using Library.LibraryService;
using Library.LibraryService.Dto;
using Library.MultiTenancy;
using Shouldly;
using Xunit;

namespace Library.Tests.LibraryService
{
    public class LibraryAppService_Tests : LibraryTestBase, IAsyncLifetime
    {
        private readonly ILibraryAppService _libraryAppService;

        public LibraryAppService_Tests()
        {
            _libraryAppService = Resolve<LibraryAppService>();
        }

        public async Task InitializeAsync()
        {
            await InjectBooksDataAsync();
            await InjectTestUser();
            await MarkTestUserAsReader();

            LoginAsTenant(Tenant.DefaultTenantName, (await FindJohnAsync()).UserName);
        }

        public async Task DisposeAsync()
        {
            // Nope
        }

        [Fact]
        public async Task GetBook_ShouldReturnCorrectlyWhenUserNotBorrowBook()
        {
            //Act
            var res = await _libraryAppService.GetBook(new GetBookInput {BookId = Book1.Id});

            //Asserts
            res.ShouldNotBeNull();
            res.Title.ShouldBe(Book1.Title);
            res.Isbn.ShouldBe(Book1.Isbn);

            res.Avaliable.ShouldBe(2);

            res.Borrowed.ShouldBe(false);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetBook_ShouldReturnCorrectlyWhenUserBorrowedTheBook(int renewTime)
        {
            var record = await InjectBorrowRecord1AndGetAsync(renewTime);

            //Act
            var res = await _libraryAppService.GetBook(new GetBookInput {BookId = Book1.Id});

            //Asserts
            res.ShouldNotBeNull();
            res.Title.ShouldBe(Book1.Title);
            res.Isbn.ShouldBe(Book1.Isbn);

            res.Avaliable.ShouldBe(1);

            res.Borrowed.ShouldBe(true);
            res.BorrowTimeLimit.ShouldBe(record.CreationTime + LibraryConsts.UserMaxBorrowDuration
                                         + LibraryConsts.RenewDuration * renewTime);
        }

        [Fact]
        public async Task GetBook_ShouldReturnCorrectlyWhenUserNotLoggined()
        {
            var record = await InjectBorrowRecord1AndGetAsync(0);
            AbpSession.UserId = null;

            //Act
            var res = await _libraryAppService.GetBook(new GetBookInput {BookId = Book1.Id});

            //Asserts
            res.ShouldNotBeNull();
            res.Title.ShouldBe(Book1.Title);
            res.Isbn.ShouldBe(Book1.Isbn);

            res.Avaliable.ShouldBe(1);

            res.Borrowed.ShouldBe(false);
        }

        [Fact]
        public async Task GetBookList_ShouldReturnCorrectly()
        {
            var record = await InjectBorrowRecord1AndGetAsync(0);

            //Act
            var res = await _libraryAppService.GetBookList();

            //Asserts
            res.Items.Count.ShouldBe(2);

            var book = res.Items.First();
            book.BorrowTimeLimit.ShouldBe(record.CreationTime + LibraryConsts.UserMaxBorrowDuration);
            book.Borrowed.ShouldBe(true);
            book.Count.ShouldBe(2);
            book.Avaliable.ShouldBe(1);
        }

        [Fact]
        public async Task GetBookList_ShouldReturnCorrectlyWhenNotLoggined()
        {
            var record = await InjectBorrowRecord1AndGetAsync(0);
            AbpSession.UserId = null;

            //Act
            var res = await _libraryAppService.GetBookList();

            //Asserts
            res.Items.Count.ShouldBe(2);

            var book = res.Items.First();
            book.Borrowed.ShouldBe(false);
            book.Count.ShouldBe(2);
            book.Avaliable.ShouldBe(1);
        }

        [Fact]
        public async Task GetUserBook_ShouldReturnCorrectly()
        {
            var record = await InjectBorrowRecord1AndGetAsync(0);
            
            //Act
            var res = await _libraryAppService.GetUserBook();

            //Asserts
            res.Items.Count.ShouldBe(1);

            var item = res.Items.First();
            item.BorrowTimeLimit.ShouldBe(record.CreationTime + LibraryConsts.UserMaxBorrowDuration);
            item.Borrowed.ShouldBe(true);
            item.Count.ShouldBe(2);
            item.Avaliable.ShouldBe(1);
        }
    }
}