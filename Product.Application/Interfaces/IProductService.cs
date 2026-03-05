using ProductService.Application.Requests;
using ProductService.Application.Responses;

namespace ProductService.Application.Interfaces
{
    public interface IProductService
    {
        Task<Guid> CreateAsync(CreateProductRequest request, Guid userId);
        Task<List<ProductResponse>> GetAllAsync();
        Task<ProductResponse?> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, UpdateProductRequest request);
        Task DeleteAsync(Guid id);
    }
}
