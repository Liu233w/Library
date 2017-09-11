using Abp.AspNetCore.Mvc.Authorization;
using Library.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : LibraryControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}