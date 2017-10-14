using Abp.AspNetCore.Mvc.Authorization;
using Library.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class HomeController : LibraryControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}