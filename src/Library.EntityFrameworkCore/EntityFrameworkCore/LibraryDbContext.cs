using Abp.Zero.EntityFrameworkCore;
using JetBrains.Annotations;
using Library.Authorization.Roles;
using Library.Authorization.Users;
using Library.BookManage;
using Library.LibraryService;
using Library.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Library.EntityFrameworkCore
{
    public class LibraryDbContext : AbpZeroDbContext<Tenant, Role, User, LibraryDbContext>
    {
        /* Define an IDbSet for each entity of the application */
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        public DbSet<Copy> Copys { get; set; }
        
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Copy>()
                .HasOne(item => item.BorrowRecord)
                .WithOne(item => item.Copy);

            modelBuilder.Entity<BorrowRecord>()
                .HasOne(item => item.Copy)
                .WithOne(item => item.BorrowRecord)
                .HasForeignKey<Copy>(item => item.BorrowRecordId)
                .IsRequired(false);
        }
    }
}
