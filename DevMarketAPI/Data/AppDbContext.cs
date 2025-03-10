using DevMarketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DevMarketAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<StudioCredentials> StudioCredentials { get; set; } // Example table

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}
