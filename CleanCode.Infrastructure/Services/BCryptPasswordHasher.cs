using CleanCodeLibrary.Application.Common.Interfaces;

namespace CleanCode.Infrastructure.Services
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public bool Verify(string plainPassword, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hash);
        }
    }
}
