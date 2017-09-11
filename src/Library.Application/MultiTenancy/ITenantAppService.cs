using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Library.MultiTenancy.Dto;

namespace Library.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
