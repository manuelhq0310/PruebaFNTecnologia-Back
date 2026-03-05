using ProductService.Domain.Models;

namespace ProductService.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<Product?> GetByIdAsync(Guid id);
        Task<List<Product>> GetAllAsync();
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
