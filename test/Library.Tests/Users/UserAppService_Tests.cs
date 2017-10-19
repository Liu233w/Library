using System.Threading.Tasks;

using Abp.Application.Services.Dto;
using Abp.UI;
using Library.LibraryService;
using Library.LibraryService.Dto;
using Library.Users;
using Library.Users.Dto;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Library.Tests.Users
{
    public class UserAppService_Tests : LibraryTestBase
    {
        private readonly IUserAppService _userAppService;
        private readonly ILibraryManageAppService _libraryManageAppService;

        public UserAppService_Tests()
        {
            _userAppService = Resolve<IUserAppService>();
            _libraryManageAppService = Resolve<LibraryManageAppService>();
        }

        [Fact]
        public async Task GetUsers_Test()
        {
            //Act
            var output = await _userAppService.GetAll(new PagedResultRequestDto {MaxResultCount = 20, SkipCount = 0});

            //Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task CreateUser_Test()
        {
            //Act
            await _userAppService.Create(
                new CreateUserDto
                {
                    EmailAddress = "john@volosoft.com",
                    IsActive = true,
                    Name = "John",
                    Surname = "Nash",
                    Password = "123qwe",
                    UserName = "john.nash"
                });

            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "john.nash");
                johnNashUser.ShouldNotBeNull();
            });
        }

        [Fact]
        public async Task Delete_ShouldActCorrectly()
        {
            await InjectTestUser();
            var john = await FindJohnAsync();
            john.IsDeleted.ShouldBe(false, "确保用户存在");
            await InjectBooksDataAsync();

            //Act
            await _userAppService.Delete(new EntityDto<long>(john.Id));

            //Asserts
            var res = await FindJohnAsync();
            res.IsDeleted.ShouldBe(true, "用户已经删除");
        }

        [Fact]
        public async Task Delete_ShouldThrowWhenUserNotReturnBook()
        {
            await InjectTestUser();
            var john = await FindJohnAsync();
            john.IsDeleted.ShouldBe(false, "确保用户存在");
            await InjectBooksDataAsync();
            await InjectBorrowRecord1AndGetAsync(0);

            //Act
            var task = _userAppService.Delete(new EntityDto<long>(john.Id));

            //Asserts
            await task.ShouldThrowAsync<UserFriendlyException>();
            var res = await FindJohnAsync();
            res.IsDeleted.ShouldBe(false);
        }

        [Fact]
        public async Task Delete_ShouldDeleteUserWhenUserReturnedAllBook()
        {
            await InjectTestUser();
            var john = await FindJohnAsync();
            john.IsDeleted.ShouldBe(false, "确保用户存在");
            await InjectBooksDataAsync();
            var record = await InjectBorrowRecord1AndGetAsync(0);

            await _libraryManageAppService.ReturnBook(new ReturnBookInput
            {
                CopyId = record.CopyId
            });

            //Act
            await _userAppService.Delete(new EntityDto<long>(john.Id));

            //Asserts
            var res = await FindJohnAsync();
            res.IsDeleted.ShouldBe(true, "用户已经删除");
        }
    }
}
