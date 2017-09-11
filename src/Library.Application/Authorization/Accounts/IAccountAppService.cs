﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Library.Authorization.Accounts.Dto;

namespace Library.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
