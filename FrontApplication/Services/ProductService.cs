using FrontApplication.Core;
using FrontApplication.Models;
using System.Net.Http.Json;

namespace FrontApplication.Services
{
    public class ProductService
    {
        private readonly HttpClient _http;

        public ProductService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient(ApiEndpoints.ProductApi);
        }

        public async Task<List<Product>> GetProducts()
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<List<Product>>>(ApiEndpoints.ProductGetAll);
            return response?.Data ?? new List<Product>();
        }

        public async Task<Product?> GetProduct(Guid id)
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<Product>>(string.Format(ApiEndpoints.ProductGetById, id));
            return response?.Data;
        }

        public async Task CreateProduct(Product product)
        {
            await _http.PostAsJsonAsync(ApiEndpoints.ProductCreate, product);
        }

        public async Task UpdateProduct(Product product)
        {
            await _http.PutAsJsonAsync(string.Format(ApiEndpoints.ProductUpdate, product.Id), product);
        }

        public async Task DeleteProduct(Guid id)
        {
            await _http.DeleteAsync(string.Format(ApiEndpoints.ProductDelete, id));
        }
    }
}
