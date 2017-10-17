using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using Library.LibraryService;
using Library.LibraryService.Dto;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Library.Tests.LibraryService
{
    public class LibraryManageAppService_Tests : LibraryTestBase, IAsyncLifetime
    {
        private readonly ILibraryManageAppService _libraryManageAppService;

        public LibraryManageAppService_Tests()
        {
            _libraryManageAppService = Resolve<LibraryManageAppService>();
        }

        [Fact]
        public async Task BorrowBook_CanBorrowBookCorrectly()
        {
            var john = await FindJohnAsync();
            //Act
            await _libraryManageAppService.BorrowBook(new BorrowBookInput
            {
                CopyId = Book1Copy1.Id,
                UserNameOrEmail = john.EmailAddress
            });

            //Asserts
            await UsingDbContextAsync(async ctx =>
            {
                var user = await FindJohnAsync();
                var record = await ctx.BorrowRecords.FirstOrDefaultAsync(
                    item => item.CopyId == Book1Copy1.Id && item.BorrowerUserId == user.Id);

                record.ShouldNotBeNull();
                record.RenewTime.ShouldBe(0);
            });
        }

        [Fact]
        public async Task BorrowBook_CanBorrowDifferentBook()
        {
            var john = await FindJohnAsync();

            //Act
            await _libraryManageAppService.BorrowBook(new BorrowBookInput
            {
                CopyId = Book1Copy1.Id,
                UserNameOrEmail = john.EmailAddress
            });
            await _libraryManageAppService.BorrowBook(new BorrowBookInput
            {
                CopyId = Book2Copy2.Id,
                UserNameOrEmail = john.EmailAddress
            });
        }

        [Fact]
        public async Task BorrowBook_CantBorrowBookTwice()
        {
            var john = await FindJohnAsync();
            await InjectBorrowRecord1AndGetAsync(0);

            // Act
            var task = _libraryManageAppService.BorrowBook(new BorrowBookInput
            {
                CopyId = Book1Copy2.Id,
                UserNameOrEmail = john.EmailAddress
            });

            // Asserts
            await task.ShouldThrowAsync<UserFriendlyException>();
        }

        [Fact]
        public async Task BorrowBook_CantBorrowCopyTwice()
        {
            var john = await FindJohnAsync();
            await InjectBorrowRecord1AndGetAsync(0);

            // Act
            var task = _libraryManageAppService.BorrowBook(new BorrowBookInput
            {
                CopyId = Book1Copy1.Id,
                UserNameOrEmail = john.EmailAddress
            });

            // Asserts
            await task.ShouldThrowAsync<UserFriendlyException>();
        }

        [Fact]
        public async Task GetBookStatus_ShouldReturnCorrectly()
        {
            var john = await FindJohnAsync();
            var record = await InjectBorrowRecord1AndGetAsync(0);

            //Act
            var result = await _libraryManageAppService.GetBookStatus(new GetBookStatusInput
            {
                BookId = Book1.Id
            });

            //Asserts
            result.Book.Isbn.ShouldBe(Book1.Isbn);
            result.Book.Avaliable.ShouldBe(1);

            result.BorrowedCopysAndRecord.Count.ShouldBe(1);
            var userRecord = result.BorrowedCopysAndRecord.First();
            userRecord.BorrowTimeLimit.ShouldBe(
                record.CreationTime + LibraryConsts.UserMaxBorrowDuration);
            userRecord.Record.CopyId.ShouldBe(Book1Copy1.Id);
            userRecord.Record.RenewedTimes.ShouldBe(0);

            userRecord.User.EmailAddress.ShouldBe(john.EmailAddress);
        }

        [Fact]
        public async Task ReturnBook_ShouldDoneCorrectly()
        {
            var record = await InjectBorrowRecord1AndGetAsync(0);

            //Act
            await _libraryManageAppService.ReturnBook(new ReturnBookInput
            {
                CopyId = Book1Copy1.Id
            });

            await UsingDbContextAsync(async ctx =>
            {
                var copy = await ctx.Copys.FindAsync(Book1Copy1.Id);
                copy.BorrowRecordId.ShouldBe(null);

                var newRecord = await ctx.BorrowRecords.FindAsync(record.Id);
                newRecord.IsDeleted.ShouldBe(true);
            });
        }

        [Fact]
        public async Task BorrowBook_CanBorrowAgainAfterReturn()
        {
            var john = await FindJohnAsync();

            //Act
            await _libraryManageAppService.BorrowBook(new BorrowBookInput
            {
                CopyId = Book1Copy1.Id,
                UserNameOrEmail = john.EmailAddress
            });
            await _libraryManageAppService.ReturnBook(new ReturnBookInput
            {
                CopyId = Book1Copy1.Id
            });
            await _libraryManageAppService.BorrowBook(new BorrowBookInput
            {
                CopyId = Book1Copy1.Id,
                UserNameOrEmail = john.EmailAddress
            });

            //Assert
            await UsingDbContextAsync(async ctx =>
            {
                (await ctx.BorrowRecords.CountAsync()).ShouldBe(2);
                var record = await ctx.BorrowRecords.FirstOrDefaultAsync(
                    item => item.IsDeleted == false && item.BorrowerUserId == john.Id);
                record.ShouldNotBeNull();

                var copy = await ctx.Copys.FindAsync(Book1Copy1.Id);
                copy.BorrowRecordId.ShouldBe(record.Id);
            });
        }

        [Fact]
        public async Task ReturnBook_ShouldThrowWhenCopyNotBorrowed()
        {
            //Act
            var task = _libraryManageAppService.ReturnBook(new ReturnBookInput
            {
                CopyId = Book1Copy1.Id
            });

            //Asserts
            await task.ShouldThrowAsync<UserFriendlyException>();
        }

        [Fact]
        public async Task ReturnBook_ShouldThrowWhenCopyReturnTwice()
        {
            var john = await FindJohnAsync();
            await _libraryManageAppService.BorrowBook(new BorrowBookInput
            {
                CopyId = Book1Copy1.Id,
                UserNameOrEmail = john.EmailAddress
            });

            //Act
            await _libraryManageAppService.ReturnBook(new ReturnBookInput
            {
                CopyId = Book1Copy1.Id
            });
            var task = _libraryManageAppService.ReturnBook(new ReturnBookInput
            {
                CopyId = Book1Copy1.Id
            });

            //Asserts
            await task.ShouldThrowAsync<UserFriendlyException>();
        }

        [Fact]
        public async Task GetOutdatedBorrowRecord_ShouldReturnCorrectly()
        {
            var record = await InjectBorrowRecord1AndGetAsync(0, true);
            var john = await FindJohnAsync();

            //Act
            var res = await _libraryManageAppService.GetOutdatedBorrowRecord();

            //Asserts
            res.Items.Count.ShouldBe(1);

            var item = res.Items.First();
            item.BookTitle.ShouldBe(Book1.Title);
            item.BorrowTimeLimit.ShouldBe(record.CreationTime + LibraryConsts.UserMaxBorrowDuration);
            item.CopyId.ShouldBe(Book1Copy1.Id);
            item.UserInfo.EmailAddress.ShouldBe(john.EmailAddress);
        }

        public async Task InitializeAsync()
        {
            await InjectBooksDataAsync();
            await InjectTestUser();
        }

        public async Task DisposeAsync()
        {
        }
    }
}