using Middleware.Exceptions;
using ProductService.Application.Interfaces;
using ProductService.Application.Requests;
using ProductService.Application.Responses;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductService.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Guid> CreateAsync(CreateProductRequest request, Guid userId)
        {
            var product = new Product(
                request.Name,
                request.Description,
                request.Category,
                request.Price,
                request.Stock,
                userId
            );

            await _productRepository.AddAsync(product);

            return product.Id;
        }

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Category = p.Category,
                Price = p.Price,
                Stock = p.Stock,
                Status = p.Status,
                CreatedByUserId = p.CreatedByUserId,
                CreatedAt = p.CreatedAt
            }).ToList();
        }

        public async Task<ProductResponse?> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price,
                Stock = product.Stock,
                Status = product.Status,
                CreatedByUserId = product.CreatedByUserId,
                CreatedAt = product.CreatedAt
            };
        }

        public async Task UpdateAsync(Guid id, UpdateProductRequest request)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new DomainException("Product not found");

            product.Update(
                request.Name,
                request.Description,
                request.Category,
                request.Price,
                request.Stock
            );

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                throw new DomainException("Product not found");

            await _productRepository.DeleteAsync(product);
        }
    }
}
