using FrontApplication.Core.Enums;

namespace FrontApplication.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ProductCategory Category { get; set; } = ProductCategory.Electronics;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public ProductStatus Status { get; set; }
    }
}
