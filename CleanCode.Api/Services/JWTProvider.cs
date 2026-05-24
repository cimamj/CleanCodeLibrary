using CleanCodeLibrary.Application.Common.Interfaces;

namespace CleanCode.Api.Services
{
    public class JWTProvider : IJWTProvider
    {
        private readonly IConfiguration _configuration;

        public JWTProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetSecretKey() => _configuration["Jwt:SecretKey"];

        public string GetIssuer() => _configuration["Jwt:Issuer"];

        public string GetAudience() => _configuration["Jwt:Audience"];

        public string GetExpiryHours() => _configuration["Jwt:ExpiryHours"] ?? "24";
    }
}
