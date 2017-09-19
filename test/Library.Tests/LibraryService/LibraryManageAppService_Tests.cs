﻿using System.Threading.Tasks;
using Library.BookManage;
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
            //Act
            await _libraryManageAppService.BorrowBook(new BorrowBookInput
            {
                BookId = 1,
                UserNameOrEmail = "john@volosoft.com"
            });

            //Asserts
            await UsingDbContextAsync(async ctx =>
            {
                var user = await FindJohnAsync();
                var record = await ctx.BorrowRecords.FirstOrDefaultAsync(
                    item => item.BookId == 1 && item.BorrowerUserId == user.Id);

                record.ShouldNotBeNull();
                record.RenewTime.ShouldBe(0);
            });
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