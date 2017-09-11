using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Library.Localization;
using Abp.Zero.Configuration;
using Library.MultiTenancy;
using Library.Authorization.Roles;
using Library.Authorization.Users;
using Library.Configuration;
using Library.Timing;

namespace Library
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class LibraryCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            LibraryLocalizationConfigurer.Configure(Configuration.Localization);

            //Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = LibraryConsts.MultiTenancyEnabled;

            //Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LibraryCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}