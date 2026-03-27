
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.Persistance.Students;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanCodeLibrary.Application.Auth.Login
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int StudentId { get; set; }
    }
    public class LoginRequestHandler : RequestHandler<LoginRequest, LoginResponse>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IConfiguration _configuration;


        public LoginRequestHandler(
        IStudentRepository studentRepository,
        IConfiguration configuration)  // ← DODAJ OVO
        {
            _studentRepository = studentRepository;
            _configuration = configuration;
        }

        protected override async Task<Result<LoginResponse>> HandleRequest(
        LoginRequest request,
        Result<LoginResponse> result)
        {

            var student = await _studentRepository.GetByEmail(request.Email);
            if (student == null || !BCrypt.Net.BCrypt.Verify(request.Password, student.PasswordHash))
            {
                result.AddError(new ValidationResultItem
                {
                    Message = "Student s ovom sifrom ne postoji u bazi, kriva sifra ili mail",
                    ValidationSeverity = ValidationSeverity.Error,
                });
                return result;
            }

            var token = GenerateJwtToken(student);

            result.SetResult(new LoginResponse
            {
                Token = token,
                Role = student.Role,
                StudentId = student.Id
            });

            return result;
        }

        private string GenerateJwtToken(Student student)
        {
            var secret = _configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("JWT SecretKey nije postavljen");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //dakle od hashiranog tajnog kljuca i ovog hmacSha algoritma se dobije creds - podaci za potpis

            var claims = new[]
            {
            new Claim("studentId", student.Id.ToString()),
            new Claim(ClaimTypes.Role, student.Role) 

        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(int.Parse(_configuration["Jwt:ExpiryHours"] ?? "24")),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
