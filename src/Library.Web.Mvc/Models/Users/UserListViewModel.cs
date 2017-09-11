using System.Collections.Generic;
using Library.Roles.Dto;
using Library.Users.Dto;

namespace Library.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}