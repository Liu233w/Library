using System.Threading.Tasks;
using Abp.Application.Services;
using Library.Sessions.Dto;

namespace Library.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
