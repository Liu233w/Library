using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Results.Wrapping;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Library.Authorization.Users;
using Library.Sessions;
using Library.Sessions.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.BookManage
{
    public class IndexModel : PageModel
    {
        public ICollection<string> List2 { get; set; }
        public GetCurrentLoginInformationsOutput CurrentLoginInformations { get; set; }

        // 使用依赖注入来初始化参数，从而使用应用层的组件
        private readonly ISessionAppService _sessionAppService;

        public IndexModel(ISessionAppService sessionAppService)
        {
            _sessionAppService = sessionAppService;
        }

        public async Task OnGetAsync()
        {
            // 方案1：动态输入参数
            ViewData["List"] = new[]
            {
                "first",
                "second",
                "third",
                "fourth"
            };

            // 方案2：采用静态的参数
            List2 = new List<string>(new[]
            {
                "item1",
                "item2",
                "item3"
            });

            // 在展现层调用应用层，产生输出
            CurrentLoginInformations = await _sessionAppService.GetCurrentLoginInformations();
        }
    }
}