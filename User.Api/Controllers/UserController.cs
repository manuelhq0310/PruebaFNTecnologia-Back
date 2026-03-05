using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Common;
using UserService.Application.Interfaces;
using UserService.Application.Requests;
using UserService.Application.Responses;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetCurrentUser()
        {
            string name = User.Identity?.Name ?? string.Empty;

            return Ok(ApiResponse<object>.SuccessResponse(name, "You are authenticated"));
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var response = await _userService.RegisterAsync(request);

            return Ok(ApiResponse<object>.SuccessResponse(response, "User registered successfully"));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _userService.LoginAsync(request);

            return Ok(ApiResponse<LoginResponse>.SuccessResponse(
                response,
                "Login successful"));
        }
    }
}
