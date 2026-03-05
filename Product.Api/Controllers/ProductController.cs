using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Common;
using ProductService.Application.Interfaces;
using ProductService.Application.Requests;
using ProductService.Domain.Models;
using System.Security.Claims;

namespace ProductService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var userId = GetUserIdFromToken();

            var productId = await _productService.CreateAsync(request, userId);

            return Ok(ApiResponse<object>.SuccessResponse(productId, "Product created successfully"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            return Ok(ApiResponse<object>.SuccessResponse(products, "Success."));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound("Product not found");

            return Ok(ApiResponse<object>.SuccessResponse(product, "Success."));
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, UpdateProductRequest request)
        {
            await _productService.UpdateAsync(id, request);

            return Ok(ApiResponse<object>.SuccessResponse(string.Empty, "Product updated successfully."));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);

            return Ok(ApiResponse<object>.SuccessResponse(string.Empty, "Product deleted successfully."));
        }

        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new Exception("UserId not found in token");

            return Guid.Parse(userIdClaim);
        }
    }
}
