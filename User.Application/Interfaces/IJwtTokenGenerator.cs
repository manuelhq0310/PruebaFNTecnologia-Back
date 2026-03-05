using UserService.Application.Responses;
using UserService.Domain.Models;

namespace UserService.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        LoginResponse GenerateToken(User user);
    }
}
