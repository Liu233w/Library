using System.Threading.Tasks;
using Abp.Application.Services;
using Library.Authorization.Accounts.Dto;
using Library.Users.Dto;

namespace Library.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        Task<UserDto> GetUserInformation();
    }
}
