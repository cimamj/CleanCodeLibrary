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
                return controller.BadRequest(response); //400 los input dakle validacijska greska, 401 unauthorized npr 
            else if (!response.HasValue) 
                return controller.NotFound(); //ne ide argument

            return controller.Ok(response); //ovo je za get npr, .Created je 201, .NoContent je 204 kad delete samo udres 
        }
    }
}
