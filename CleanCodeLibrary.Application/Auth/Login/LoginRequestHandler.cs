using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation.ValidationItems;
using CleanCodeLibrary.Domain.Entities.Students;
using CleanCodeLibrary.Domain.Persistance.Students;
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
        private readonly IJWTProvider _jwtProvider;
        private readonly IPasswordHasher _passwordHasher;

        public LoginRequestHandler(
            IStudentRepository studentRepository,
            IJWTProvider jwtProvider,
            IPasswordHasher passwordHasher)
        {
            _studentRepository = studentRepository;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
        }

        protected override async Task<Result<LoginResponse>> HandleRequest(LoginRequest request, Result<LoginResponse> result)
        {
            var student = await _studentRepository.GetByEmail(request.Email);

            if (student == null || !_passwordHasher.Verify(request.Password, student.PasswordHash))
            {
                result.AddError(ValidationResultItem.FromValidationItem(ValidationItems.Student.LoginFailed));

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
            var secret = _jwtProvider.GetSecretKey();
            var issuer = _jwtProvider.GetIssuer();
            var audience = _jwtProvider.GetAudience();
            var expiryHours = int.Parse(_jwtProvider.GetExpiryHours());

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("studentId", student.Id.ToString()),
                new Claim(ClaimTypes.Role, student.Role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(expiryHours),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
