using Microsoft.EntityFrameworkCore;
using Webshop.Domain.Entities;

namespace WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // StockItems table - only table in the stock API database
        public DbSet<StockItem> StockItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // No FK to Product — Product lives in a different database
            modelBuilder.Entity<StockItem>(entity =>
            {
                entity.HasKey(s => s.StockItemId);
                entity.Property(s => s.ProductId).IsRequired();
                entity.HasIndex(s => s.ProductId);
            });
        }
    }
}

