using System.Net;
using System.Text.Json;

namespace CleanCode.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = GetStatusCode(ex);

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
                isAuthorized = statusCode != 401,
                hasError = true,
                value = (object?)null
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private static int GetStatusCode(Exception ex) => ex switch
        {
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        private static string GetMessage(Exception ex) => ex switch
        {
            UnauthorizedAccessException => "Nemate pristup ovom resursu",
            ArgumentException e => e.Message,
            KeyNotFoundException e => e.Message,
            NullReferenceException => "Interni error - objekt nije pronađen",
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
