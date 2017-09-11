using System.Reflection;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Library.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Library.Web.Host.Startup
{
    [DependsOn(
       typeof(LibraryWebCoreModule))]
    public class LibraryWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public LibraryWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LibraryWebHostModule).GetAssembly());
        }
    }
}
