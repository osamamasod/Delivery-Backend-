using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using FoodDelivery.DAL.Data;

namespace FoodDelivery.DAL
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Use your connection string from appsettings.json directly
            var connectionString = "Host=localhost;Port=5432;Database=myDB;Username=osamamasoud;";
            
            optionsBuilder.UseNpgsql(connectionString);
            
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
