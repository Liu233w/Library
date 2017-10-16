using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Library.EntityFrameworkCore
{
    public static class LibraryDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<LibraryDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<LibraryDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}