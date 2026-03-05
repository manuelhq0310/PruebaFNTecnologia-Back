using UserService.Application.Requests;
using UserService.Application.Responses;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);
        Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    }
}
