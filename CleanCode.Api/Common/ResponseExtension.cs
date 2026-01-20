using CleanCodeLibrary.Application.Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace CleanCode.Api.Common
{
    public static class ResponseExtension
    {
        public static ActionResult ToActionResult<TValue>(this Result<TValue> result, ControllerBase controller) where TValue : class
        {
            var response = new Response<TValue>(result);

            if (response.HasError)
                return controller.BadRequest(response);

            return controller.Ok(response);
        }
    }
}
