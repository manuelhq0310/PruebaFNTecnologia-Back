using Middleware.Exceptions;
using Moq;
using ProductService.Application.Requests;
using ProductService.Domain.Enums;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;
using ApplicationLayer = ProductService.Application.Services;

namespace ProductService.Test
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly ApplicationLayer.ProductService _service;

        public ProductServiceTest()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _service = new ApplicationLayer.ProductService(_repositoryMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProductAndReturnId()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "Laptop",
                Description = "Gaming laptop",
                Category = ProductCategory.Electronics,
                Price = 2000,
                Stock = 10
            };

            var userId = Guid.NewGuid();

            // Act
            var result = await _service.CreateAsync(request, userId);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            _repositoryMock.Verify(
                r => r.AddAsync(It.IsAny<Product>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product("Laptop","Gaming",ProductCategory.Electronics,2000,5,Guid.NewGuid()),
                new Product("Phone","Smartphone",ProductCategory.Electronics,800,10,Guid.NewGuid())
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result.First().Name);
        }

        [Fact]
        public async Task GetByIdAsync_WhenProductDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_WhenProductExists()
        {
            // Arrange
            var product = new Product("Laptop", "Gaming", ProductCategory.Electronics, 2000, 5, Guid.NewGuid());

            var request = new UpdateProductRequest
            {
                Name = "Laptop Pro",
                Description = "Updated",
                Category = ProductCategory.Electronics,
                Price = 2500,
                Stock = 8
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            // Act
            await _service.UpdateAsync(product.Id, request);

            // Assert
            _repositoryMock.Verify(
                r => r.UpdateAsync(product),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenProductDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = new UpdateProductRequest
            {
                Name = "Laptop",
                Description = "Gaming",
                Category = ProductCategory.Electronics,
                Price = 2000,
                Stock = 5
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() =>
                _service.UpdateAsync(id, request));

            Assert.Equal("Product not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product("Laptop", "Gaming", ProductCategory.Electronics, 2000, 5, Guid.NewGuid());

            _repositoryMock
                .Setup(r => r.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            // Act
            await _service.DeleteAsync(product.Id);

            // Assert
            _repositoryMock.Verify(
                r => r.DeleteAsync(product),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenProductDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() =>
                _service.DeleteAsync(id));

            Assert.Equal("Product not found", exception.Message);
        }
    }
}