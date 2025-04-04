using DevMarketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DevMarketAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<StudioCredentials> StudioCredentials { get; set; } 
        public DbSet<StudioProfile> StudioProfiles { get; set; }
        public DbSet<Dev> Devs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<TradingStatus> TradingStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}
