using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Library.Authorization;
using Library.LibraryService;
using Library.LibraryService.Dto;

namespace Library
{
    [DependsOn(
        typeof(LibraryCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class LibraryApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<LibraryAuthorizationProvider>();
        }

        public override void Initialize()
        {
            Assembly thisAssembly = typeof(LibraryApplicationModule).GetAssembly();
            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                //Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg.AddProfiles(thisAssembly);

                //Add maps
                cfg.CreateMap<BorrowRecord, BorrowRecordWithAdditionalInfo>()
                    .ForMember(dest => dest.BookTitle, opts => opts.MapFrom(
                        src => src.Book.Title))
                    .ForMember(dest => dest.BorrowTimeLimit, opts => opts.MapFrom(
                        src => src.GetOutdatedTime()));
            });
        }
    }
}