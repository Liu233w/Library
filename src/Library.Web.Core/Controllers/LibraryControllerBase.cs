using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Library.Controllers
{
    public abstract class LibraryControllerBase: AbpController
    {
        protected LibraryControllerBase()
        {
            LocalizationSourceName = LibraryConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}