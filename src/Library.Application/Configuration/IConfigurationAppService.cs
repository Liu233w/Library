using System.Threading.Tasks;
using Library.Configuration.Dto;

namespace Library.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}