using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Library.Roles.Dto;
using Library.Users.Dto;

namespace Library.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task MarkUserAsLibraryManagerByUserNameOrEmail(UserNameOrEmailInput input);
    }
}