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

        // ʹ������ע������ʼ���������Ӷ�ʹ��Ӧ�ò�����
        private readonly ISessionAppService _sessionAppService;

        public IndexModel(ISessionAppService sessionAppService)
        {
            _sessionAppService = sessionAppService;
        }

        public async Task OnGetAsync()
        {
            // ����1����̬�������
            ViewData["List"] = new[]
            {
                "first",
                "second",
                "third",
                "fourth"
            };

            // ����2�����þ�̬�Ĳ���
            List2 = new List<string>(new[]
            {
                "item1",
                "item2",
                "item3"
            });

            // ��չ�ֲ����Ӧ�ò㣬�������
            CurrentLoginInformations = await _sessionAppService.GetCurrentLoginInformations();
        }
    }
}