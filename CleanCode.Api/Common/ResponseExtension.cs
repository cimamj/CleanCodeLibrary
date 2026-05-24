using CleanCodeLibrary.Application.Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace CleanCode.Api.Common
{
    public static class ResponseExtension
    {
        public static ActionResult ToActionResult<TValue>(this Result<TValue> result, ControllerBase controller) where TValue : class
        {
            var response = new Response<TValue>(result);

            if (!result.IsAuthorized)
            {
                return controller.StatusCode(403, response);
            }

            if (response.HasError)
            {
                if (response.Errors.Any(e => e.ValidationType == CleanCodeLibrary.Domain.Common.Validation.ValidationType.NotFound))
                    return controller.NotFound(response);

                return controller.BadRequest(response);
            }

            if (!response.HasValue)
            {
                return controller.NotFound(response);
            }

            var method = controller.HttpContext.Request.Method;

            return method switch
            {
                "POST" => controller.CreatedAtAction(
                    null,
                    new
                    {
                        id = response.Value
                    },
                    response
                ),
                "DELETE" => controller.NoContent(),
                "PUT" or "PATCH" => controller.Ok(response),
                _ => controller.Ok(response)
            };
        }
    }
}
