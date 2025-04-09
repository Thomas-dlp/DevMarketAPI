using DevMarketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DevMarketAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<StudioCredentials> StudioCredentials { get; set; } 
        public DbSet<StudioProfile> StudioProfiles { get; set; }
        public DbSet<DisplayableElementReferenceLink> ReferenceLinks { get; set; }
        public DbSet<Dev> Devs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<TradingStatus> TradingStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure DisplayableElementReferenceLink
            modelBuilder.Entity<DisplayableElementReferenceLink>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<DisplayableElementReferenceLink>()
                .HasIndex(r => r.StudioId);

            modelBuilder.Entity<DisplayableElementReferenceLink>()
                .HasIndex(r => new { r.StudioId, r.DisplayableElementId, r.DisplayableElementType })
                .IsUnique();

            // Example: Using enum conversion to string (optional)
            modelBuilder.Entity<DisplayableElementReferenceLink>()
                .Property(r => r.DisplayableElementType)
                .HasConversion<string>()
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }

}
