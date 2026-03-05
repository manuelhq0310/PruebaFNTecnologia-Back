using ProductService.Domain.Enums;

namespace ProductService.Domain.Models
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public ProductCategory Category { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public ProductStatus Status { get; private set; }
        public Guid CreatedByUserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Product() { }

        public Product(
            string name,
            string description,
            ProductCategory category,
            decimal price,
            int stock,
            Guid createdByUserId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Category = category;
            Price = price;
            Stock = stock;
            Status = stock > 0 ? ProductStatus.Active : ProductStatus.OutOfStock;
            CreatedByUserId = createdByUserId;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(
            string name,
            string description,
            ProductCategory category,
            decimal price,
            int stock)
        {
            Name = name;
            Description = description;
            Category = category;
            Price = price;
            Stock = stock;
            UpdatedAt = DateTime.UtcNow;

            Status = stock > 0
                ? ProductStatus.Active
                : ProductStatus.OutOfStock;
        }

        public void ChangeStatus(ProductStatus status)
        {
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
