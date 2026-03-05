using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Models;

namespace ProductService.Infrastructure.DB.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.Stock)
                .IsRequired();

            builder.Property(p => p.Category)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property(p => p.CreatedByUserId)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedAt);
        }
    }
}
