using Abp.Zero.EntityFrameworkCore;
using Library.Authorization.Roles;
using Library.Authorization.Users;
using Library.BookManage;
using Library.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Library.EntityFrameworkCore
{
    public class LibraryDbContext : AbpZeroDbContext<Tenant, Role, User, LibraryDbContext>
    {
        /* Define an IDbSet for each entity of the application */
        public DbSet<Book> Books { get; set; }
        
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {

        }
    }
}
