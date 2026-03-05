using ProductService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Application.Requests
{
    public class CreateProductRequest
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = default!;

        [Required]
        public ProductCategory Category { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public int Stock { get; set; }
    }
}
