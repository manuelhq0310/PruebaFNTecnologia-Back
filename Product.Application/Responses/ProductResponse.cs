using ProductService.Domain.Enums;

namespace ProductService.Application.Responses
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ProductCategory Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public ProductStatus Status { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
