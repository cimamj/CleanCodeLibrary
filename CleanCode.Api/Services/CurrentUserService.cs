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

        private ClaimsPrincipal User => _httpContextAccessor.HttpContext.User; //umisto u konstruktoru ode odma postane, polje varijabla obicna
        public bool IsAuthenticated() => User?.Identity?.IsAuthenticated ?? false; //metoda

        public int? GetStudentId()
        {
            var idSt = User.Claims.FirstOrDefault(x => x.Type == "studentId");
            if (idSt == null)
                return null;
            if (int.TryParse(idSt.Value, out int id))
                return id;
            return null;
        }//nullabilan jer neki tokeni nece imati id,
        //mozes maknuti ? i provjeriti je li null, baciti Exception, ako nije odmah int.parse sigurno ce proci jer u claimu znas da imas 
        //ali ako imas 100 entiteta i neznas in sve tokene, ovo je ipak sigurnije, i generi

        public string GetRole()
        {
            var role = User.FindFirst(ClaimTypes.Role); //ugradena metoda
            return role?.Value;
            //var role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role); //.Contains("Admin")
            //return role?.Value;            
        }

        public bool IsAdmin()
        {
            var role = User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            return role != null && role.Value == "Admin";
        }


    }
}
