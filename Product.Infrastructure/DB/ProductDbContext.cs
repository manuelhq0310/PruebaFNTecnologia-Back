using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Models;

namespace ProductService.Infrastructure.DB
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
