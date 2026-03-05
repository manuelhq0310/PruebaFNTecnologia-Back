namespace ProductService.Infrastructure.Authentication
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
    }
}
