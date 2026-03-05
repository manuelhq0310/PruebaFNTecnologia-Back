using Microsoft.Extensions.Logging;
using Middleware.Exceptions;
using UserService.Application.Interfaces;
using UserService.Application.Requests;
using UserService.Application.Responses;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        public async Task<RegisterUserResponse> RegisterAsync(
            RegisterUserRequest request,
            CancellationToken cancellationToken = default)
        {
            var emailNormalized = request.Email.Trim().ToLower();

            var exists = await _userRepository
                .ExistsByEmailAsync(emailNormalized, cancellationToken);

            if (exists)
            {
                _logger.LogWarning("Attempt to register with existing email: {Email}", emailNormalized);
                throw new DomainException("Email already exists.");
            }

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = User.Create(
                request.Name,
                emailNormalized,
                passwordHash,
                request.Role
            );

            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User registered successfully: {UserId}", user.Id);

            return new RegisterUserResponse()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<LoginResponse> LoginAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            var emailNormalized = request.Email.Trim().ToLower();

            var user = await _userRepository
                .GetByEmailAsync(emailNormalized, cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("Login failed. User not found: {Email}", emailNormalized);
                throw new DomainException("Invalid credentials.");
            }

            var validPassword = _passwordHasher
                .Verify(request.Password, user.PasswordHash);

            if (!validPassword)
            {
                _logger.LogWarning("Login failed. Invalid password for: {Email}", emailNormalized);
                throw new DomainException("Invalid credentials.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            _logger.LogInformation("User logged in successfully: {UserId}", user.Id);

            return token;
        }
    }
}
