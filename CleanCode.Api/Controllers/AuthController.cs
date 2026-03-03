using CleanCode.Api.Common;
using CleanCodeLibrary.Application.Auth.Login;
using CleanCodeLibrary.Domain.Persistance.Students;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace CleanCode.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        [HttpPost("login")]
        public async Task<ActionResult> Login(
            [FromBody] CleanCodeLibrary.Application.Auth.Login.LoginRequest request,
            [FromServices] IStudentRepository studentRepository,
            [FromServices] IConfiguration configuration
            )
        {
            var handler = new CleanCodeLibrary.Application.Auth.Login.LoginRequestHandler(studentRepository, configuration);
            var result = await handler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }
    }
}
