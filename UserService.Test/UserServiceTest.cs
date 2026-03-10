using Microsoft.Extensions.Logging;
using Middleware.Exceptions;
using Moq;
using UserService.Application.Interfaces;
using UserService.Application.Requests;
using UserService.Application.Responses;
using UserService.Domain.Enums;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using ApplicationLayer = UserService.Application.Services;

namespace UserService.Test
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private readonly Mock<ILogger<ApplicationLayer.UserService>> _loggerMock;

        private readonly ApplicationLayer.UserService _service;

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            _loggerMock = new Mock<ILogger<ApplicationLayer.UserService>>();

            _service = new ApplicationLayer.UserService(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenGeneratorMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_Success()
        {
            // Arrange
            var request = new RegisterUserRequest
            {
                Name = "John",
                Email = "john@email.com",
                Password = "123456",
                Role = UserRole.User
            };

            var passwordHash = "hashedPassword";

            _userRepositoryMock
                .Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _passwordHasherMock
                .Setup(h => h.Hash(request.Password))
                .Returns(passwordHash);

            // Act
            var result = await _service.RegisterAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.Email.ToLower(), result.Email);
            Assert.Equal(request.Role.ToString(), result.Role);

            _userRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _userRepositoryMock.Verify(
                r => r.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WhenEmailAlreadyExists()
        {
            // Arrange
            var request = new RegisterUserRequest
            {
                Name = "John",
                Email = "john@email.com",
                Password = "123456",
                Role = UserRole.User
            };

            _userRepositoryMock
                .Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() =>
                _service.RegisterAsync(request));

            Assert.Equal("Email already exists.", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_Success()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "user@email.com",
                Password = "123456"
            };

            var user = User.Create(
                "User",
                "user@email.com",
                "hashedPassword",
                UserRole.User
            );

            var tokenResponse = new LoginResponse
            {
                Token = "jwt_token"
            };

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(p => p.Verify(request.Password, user.PasswordHash))
                .Returns(true);

            _jwtTokenGeneratorMock
                .Setup(j => j.GenerateToken(user))
                .Returns(tokenResponse);

            // Act
            var result = await _service.LoginAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("jwt_token", result.Token);
        }

        [Fact]
        public async Task LoginAsync_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "user@email.com",
                Password = "123456"
            };

            _userRepositoryMock
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() =>
                _service.LoginAsync(request));

            Assert.Equal("Invalid credentials.", exception.Message);
        }
    }
}