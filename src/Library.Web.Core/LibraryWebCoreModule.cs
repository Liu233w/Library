﻿using System;
using System.IO;
using System.Reflection;
using System.Text;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using Library.Authentication.JwtBearer;
using Library.Configuration;
using Library.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

#if FEATURE_SIGNALR
using Abp.Web.SignalR;
#endif

namespace Library
{
    [DependsOn(
         typeof(LibraryApplicationModule),
         typeof(LibraryEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
#if FEATURE_SIGNALR 
        ,typeof(AbpWebSignalRModule)
#endif
     )]
    public class LibraryWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public LibraryWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                LibraryConsts.ConnectionStringName
            );

            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(LibraryApplicationModule).GetAssembly()
                 );

            ConfigureTokenAuth();
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LibraryWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            SetAppFoldersConfig();
        }

        private void SetAppFoldersConfig()
        {
            var appFolders = IocManager.Resolve<AppFoldersConfiguration>();
            appFolders.AppDataFolder = Path.Combine(_env.ContentRootPath, "App_Data");
            appFolders.UploadFolder = Path.Combine(appFolders.AppDataFolder, @"Upload\Common"); // 指定目录 \App_Data\Upload\Common\
            appFolders.Attachments = Path.Combine(appFolders.AppDataFolder, @"Upload\Attachments"); // 指定目录 \App_Data\Upload\Attachments\
            appFolders.WebLogsFolder = Path.Combine(appFolders.AppDataFolder, @"Logs");    // 指定目录 \App_Data\Logs\
        }
    }
}
