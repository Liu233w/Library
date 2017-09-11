using Abp.AspNetCore.Mvc.ViewComponents;

namespace Library.Web.Views
{
    public abstract class LibraryViewComponent : AbpViewComponent
    {
        protected LibraryViewComponent()
        {
            LocalizationSourceName = LibraryConsts.LocalizationSourceName;
        }
    }
}