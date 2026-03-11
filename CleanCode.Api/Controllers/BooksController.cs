using Microsoft.AspNetCore.Mvc;
using CleanCodeLibrary.Application.Books.Book;
using CleanCodeLibrary.Application.Common.Model;
using CleanCode.Api.Common;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Books;
using Microsoft.AspNetCore.Authorization;
using CleanCode.Api.Services;
using CleanCodeLibrary.Application.Common.Interfaces;

namespace CleanCode.Api.Controllers
{
    [Route("api/books")] //kad .net vidi ovo, DI stvara new StudentsController(), .net poziva njegove metode controller.Post(...)
    [ApiController]
    public class BooksController : ControllerBase
    {

        // POST IMAMO
        [Authorize] //necemo stavit roles=admin nego u authorization to implementirati 
        [HttpPost] 
        public async Task<ActionResult> Post( //actionresult je tip povratne vrijednosti, ok, bad, vidis u ext, KAZE CLAUDE DA ODE IDE CREATEBOOK...HANDLER A NE OVO, DI RJESAVA SVE, DA JA RUCNO INSTACIRAM HANDLER DA JE TO LSOE
                [FromServices] IBookRepository bookRepository, //.net ubacuje instacu iz addscoped, iz DI kontejnera
                [FromBody] CreateBookRequest request, //uzmi iz tijela http zahtjeva { "firstName": "Jure", "lastName": "Horvat" }, .NET TO AUTOMATSKI DESERIALIZIRA U OBJEKT CreateStudentRequest DTO taj
                [FromServices] ICurrentUserService currentUser
                ) //from body iz {} izvuce
        {
            var requestHandler = new CreateBookRequestHandler(bookRepository, currentUser);
            var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this); //this je instaca ovog controllera automatski se radi 
                                                //urednije nego ResponseExtension.ToActionResult(result, this); 
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")] 
        public async Task<ActionResult> Update(
                [FromRoute] int id,
                [FromServices] IBookRepository bookRepository,
                [FromBody] UpdateBookRequest request
            )
        {
            // Ako trebaš ID iz rute provjeriti s requestom – možeš dodati validaciju ???? ne kuzim
            request.Id = id; //dolazi iz routt ne body
            var requestHandler = new UpdateBookRequestHandler(bookRepository);
            var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(
                [FromRoute] int id,
                [FromServices] IBookRepository bookRepository
                )
        {
            var request = new DeleteBookRequest { Id = id };
            var requestHandler = new DeleteBookRequestHandler(bookRepository);
            var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get(
            [FromServices] IBookRepository bookRepository,
            [FromServices] IBookCacheService cacheService
            )
        {
            var request = new GetAllBooksRequest(); //
            var requestHandler = new GetAllBooksRequestHandler(bookRepository, cacheService);
            var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(
            [FromRoute] int id, //radilo i from body
            [FromServices] IBookRepository bookRepository
        )
        {
            var request = new GetByIdRequest { Id = id }; //ne iz bodya?
            var handler = new GetByIdRequestHandler(bookRepository);
            var result = await handler.ProcessAuthorizedRequestAsync(request);

            return result.ToActionResult(this);
        }

        // [HttpDelete("{id}")] //ova metoda doli reagira samo na POST
        // public async Task<ActionResult> Delete(
        //    [FromRoute] int id,
        //    [FromServices] IStudentRepository studentRepository
        //)
        // {
        //     var request = new DeleteStudentRequest { Id = id };
        //     var requestHandler = new DeleteStudentRequestHandler(studentRepository);
        //     var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
        //     return result.ToActionResult(this);
        // }


        //[HttpGet]
        //public async Task<ActionResult> Get(
        //      [FromServices] IStudentRepository studentRepository
        //    //  [FromBody] GetAllStudentsRequestHandler request  //u bodyu nemam nista, ali handle prima ovaj request, moram ga imati
        //    )
        //{
        //    var request = new GetAllStudentsRequest(); //instaciramo nista praznu klasu
        //    var requestHandler = new GetAllStudentsRequestHandler(studentRepository);
        //    var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
        //    return result.ToActionResult(this);
        //}



    }
}