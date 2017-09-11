using System.Reflection;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Library.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Library.Web.Startup
{
    [DependsOn(typeof(LibraryWebCoreModule))]
    public class LibraryWebMvcModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public LibraryWebMvcModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<LibraryNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LibraryWebMvcModule).GetAssembly());
        }
    }
}