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

                return controller.BadRequest(response); //400
            }

            if (!response.HasValue)
            {
                return controller.NotFound(response); //404 ja dosad vraca ovo 

            }


            //ako je sve ok 200 201 204 ovisi sto je u bodyu je li put post delete
            var method = controller.HttpContext.Request.Method; //metoda van bodya

            return method switch
            {
                "POST" => controller.CreatedAtAction( // 201 create
                    null,                     
                    new { id = response.Value },    //id di je ruta                         
                    response
                ),
                "DELETE" => controller.NoContent(), //204
                "PUT" or "PATCH" => controller.Ok(response), 
                _ => controller.Ok(response)           //default
            };
            //klasicni switch case sce sprema u reustl neki pas e iza svega returna

        }

        //sad je to to???
    }
}
