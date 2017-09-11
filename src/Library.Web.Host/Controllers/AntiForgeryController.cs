using Library.Controllers;
using Microsoft.AspNetCore.Antiforgery;

namespace Library.Web.Host.Controllers
{
    public class AntiForgeryController : LibraryControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}