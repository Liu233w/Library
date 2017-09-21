using System.Threading.Tasks;
using Castle.MicroKernel;
using Library.LibraryService;
using Library.LibraryService.Dto;
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

            LoginAsHost((await FindJohnAsync()).UserName);
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

            res.Avaliable.ShouldBe(Book1.Count);

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

            res.Avaliable.ShouldBe(Book1.Count - 1);

            res.Borrowed.ShouldBe(true);
            res.BorrowTimeLimit.ShouldBe(record.CreationTime + LibraryConsts.UserMaxBorrowDuration
                                         + LibraryConsts.RenewDuration * renewTime);
        }
    }
}