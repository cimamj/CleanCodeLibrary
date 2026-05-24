using CleanCode.Api.Common;
using CleanCodeLibrary.Application.Auth.Login;
using Microsoft.AspNetCore.Mvc;

namespace CleanCode.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginRequestHandler _loginHandler;

        public AuthController(LoginRequestHandler loginHandler)
        {
            _loginHandler = loginHandler;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _loginHandler.ProcessAuthorizedRequestAsync(request);

            return result.ToActionResult(this);
        }
    }
}
