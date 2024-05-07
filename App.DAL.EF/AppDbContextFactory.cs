using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF
{
    public class AppDbContextFactory
    {
        public class BloggingContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Initial Catalog=localdb;Integrated Security=true");

                return new AppDbContext(optionsBuilder.Options);
            }
        }
    }
}
