using CleanCodeLibrary.Application.Common.Interfaces;
using System.Security.Claims;

namespace CleanCode.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;

        public bool IsAuthenticated() => User?.Identity?.IsAuthenticated ?? false;

        public int? GetStudentId()
        {
            var value = User.FindFirst("studentId")?.Value;

            return int.TryParse(value, out int id) ? id : null;
        }

        public string GetRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }

        public bool IsAdmin()
        {
            var role = User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

            return role != null && role.Value == "Admin";
        }
    }
}
