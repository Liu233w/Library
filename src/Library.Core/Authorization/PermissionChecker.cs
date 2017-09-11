using Abp.Authorization;
using Library.Authorization.Roles;
using Library.Authorization.Users;

namespace Library.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
