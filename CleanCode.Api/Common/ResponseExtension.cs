using CleanCodeLibrary.Application.Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace CleanCode.Api.Common
{
    public static class ResponseExtension
    {
        public static ActionResult ToActionResult<TValue>(this Result<TValue> result, ControllerBase controller) where TValue : class
        {
            var response = new Response<TValue>(result);

            if(!response.isAuthorized)
            {
                return controller.Unauthorized(new
                {
                    message = "Nemate dozvolu za ovu akciju",
                    requestId = result.RequestId
                }); // jer response nisam napunio validacijom za unatuhorized
            }// 401 Unauthorized

            if (response.HasError) 
            {
                if(!response.HasValue)
                    return controller.NotFound(response); //404

                return controller.BadRequest(response); //400
            }
         

            return controller.Ok(response); //ovo je za get npr, .Created je 201, .NoContent je 204 kad delete samo udres 
        }


    }
}
