using System.Threading.Tasks;
using Abp.Configuration.Startup;
using Library.LibraryService;
using Library.LibraryService.Dto;
using Library.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Views.Shared.Components.SideBarUserArea
{
    public class SideBarUserAreaViewComponent : LibraryViewComponent
    {
        private readonly ISessionAppService _sessionAppService;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ILibraryAppService _libraryAppService;

        public SideBarUserAreaViewComponent(ISessionAppService sessionAppService,
            IMultiTenancyConfig multiTenancyConfig, ILibraryAppService libraryAppService)
        {
            _sessionAppService = sessionAppService;
            _multiTenancyConfig = multiTenancyConfig;
            _libraryAppService = libraryAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userPhotoSrc = "~/images/user.png";
            if (AbpSession.UserId.HasValue)
            {
                var photo = await _libraryAppService.GetUserPhoto(new GetUserPhotoInput
                {
                    UserId = null
                });
                if (photo.PhotoId.HasValue)
                {
                    userPhotoSrc = "/Uploads/GetFile?id=" + photo.PhotoId.Value;
                }
            }

            var model = new SideBarUserAreaViewModel
            {
                LoginInformations = await _sessionAppService.GetCurrentLoginInformations(),
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                UserPhotoSrc = userPhotoSrc,
            };

            return View(model);
        }
    }
}
