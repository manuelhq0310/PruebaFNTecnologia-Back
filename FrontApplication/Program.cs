using Blazored.LocalStorage;
using FrontApplication;
using FrontApplication.Core;
using FrontApplication.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddTransient<AuthHttpMessageHandler>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<JwtAuthenticationStateProvider>());

builder.Services.AddHttpClient(ApiEndpoints.UserApi, client =>
{
    client.BaseAddress = new Uri(ApiEndpoints.UserBaseAddress);
})
.AddHttpMessageHandler<AuthHttpMessageHandler>();

builder.Services.AddHttpClient(ApiEndpoints.ProductApi, client =>
{
    client.BaseAddress = new Uri(ApiEndpoints.ProductBaseAddress);
})
.AddHttpMessageHandler<AuthHttpMessageHandler>();

builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    return factory.CreateClient(ApiEndpoints.UserApi);
});

builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    return factory.CreateClient(ApiEndpoints.ProductApi);
});



await builder.Build().RunAsync();
