using System.Net.Http.Json;
using Blazored.LocalStorage;
using FrontApplication.Core;
using FrontApplication.Models;

namespace FrontApplication.Services;

public class UserService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public UserService(IHttpClientFactory factory, ILocalStorageService localStorage)
    {
        _http = factory.CreateClient(ApiEndpoints.UserApi);
        _localStorage = localStorage;
    }

    public async Task<ApiResponse<LoginResponse>?> Login(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync(ApiEndpoints.UserLogin, request);

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();

        return result;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("jwt");
    }

    public async Task<string> GetToken()
    {
        return await _localStorage.GetItemAsync<string>("jwt");
    }

    public async Task<bool> Register(RegisterRequest request)
    {
        var response = await _http.PostAsJsonAsync(ApiEndpoints.UserRegister, request);

        return response.IsSuccessStatusCode;
    }
}
