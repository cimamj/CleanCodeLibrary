using System.Net;
using System.Text.Json;

namespace CleanCode.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next; // tip funkcije je ovaj next, doslovno iduci middleware i rposljeduje se httpcontext

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // pozovi sve ispod - auth, controller, handler
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            //logiraj gresku, kasnije mos dodat ILogger
            
            context.Response.StatusCode = GetStatusCode(ex);

            //var isAuthorized = ex is not UnauthorizedAccessException;
            var statusCode = GetStatusCode(ex);

            var response = new
            {
                errors = new[]
                {
                    new
                    {
                        code = GetCode(ex),
                        message = GetMessage(ex),
                        validationSeverity = "Error"
                    }
                },
                isAuthorized = statusCode != 401, //ovo radi samo za excp, nece pogodit [Authorize]
                hasError = true,
                value = (object?)null
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private static int GetStatusCode(Exception ex) => ex switch
        {
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,    // 401, ovo je pokriveno vec sa [authorize] ili isAuthorized funkcijom koja ili puni response ili baca odg
            ArgumentException => (int)HttpStatusCode.BadRequest,      // 400 pokriveno svi slucajevi jedino ako napisem throw new ArgumentExcepiton...
            KeyNotFoundException => (int)HttpStatusCode.NotFound,        // 404 pokriveno
            _ => (int)HttpStatusCode.InternalServerError // 500
        };

        private static string GetMessage(Exception ex) => ex switch
        {
            UnauthorizedAccessException => "Nemate pristup ovom resursu", //Ode nema varijable i ne treba jer je samo tekst
            ArgumentException e => e.Message, //sa varijablom e, se u returnu moze iskoristiti ista i njeno polje message
            KeyNotFoundException e => e.Message,
            NullReferenceException => "Interni error - objekt nije pronađen", //vise manje samo radi ovog napisano i napravljeno 
            _ => "Interni server error"
        };

        private static string GetCode(Exception ex) => ex switch
        {
            UnauthorizedAccessException => "UNAUTHORIZED",
            ArgumentException => "BAD_REQUEST",
            KeyNotFoundException => "NOT_FOUND",
            NullReferenceException => "NULL_REFERENCE",
            _ => "INTERNAL_ERROR"
        };
    }
}