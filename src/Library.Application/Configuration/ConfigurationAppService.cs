using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Library.Configuration.Dto;

namespace Library.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : LibraryAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
