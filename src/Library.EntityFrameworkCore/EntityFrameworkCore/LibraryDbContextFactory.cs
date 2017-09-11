using Library.Configuration;
using Library.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Library.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
    {
        public LibraryDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LibraryDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            LibraryDbContextConfigurer.Configure(builder, configuration.GetConnectionString(LibraryConsts.ConnectionStringName));

            return new LibraryDbContext(builder.Options);
        }
    }
}