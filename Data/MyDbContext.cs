using Microsoft.EntityFrameworkCore;
using EFCore.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Design;
using EFCore;

namespace EfCore.Data
{
    public class MyAppContextFactory : IDesignTimeDbContextFactory<MyAppContext>
    {
        public MyAppContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyAppContext>();
            optionsBuilder.UseSqlite(Constants.ConnectionString);

            return new MyAppContext(optionsBuilder.Options);
        }
    }

    public class MyAppContext : DbContext
    {
        public DbSet<Cat> Cat { get; set; }
        public DbSet<CatBreed> CatBreed { get; set; }

        public MyAppContext(DbContextOptions<MyAppContext> options)
            : base(options)
        { }
    }
}