using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Library.Web.Views
{
    public abstract class LibraryRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected LibraryRazorPage()
        {
            LocalizationSourceName = LibraryConsts.LocalizationSourceName;
        }
    }
}
