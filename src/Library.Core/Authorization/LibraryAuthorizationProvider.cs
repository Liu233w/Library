using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Library.Authorization
{
    public class LibraryAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            context.CreatePermission(PermissionNames.Pages_BookManage);
            context.CreatePermission(PermissionNames.Pages_LibraryManage);
            context.CreatePermission(PermissionNames.Pages_Library);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LibraryConsts.LocalizationSourceName);
        }
    }
}
