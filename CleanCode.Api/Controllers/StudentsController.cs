using Microsoft.AspNetCore.Mvc;
using CleanCodeLibrary.Application.Students.Student;
using CleanCodeLibrary.Application.Common.Model;
using CleanCode.Api.Common;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Students;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using CleanCode.Api.Services;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.DTOs.Students;

namespace CleanCode.Api.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //private static string GetTokenFromRequest(HttpRequest request)
        //{
        //    return request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        //}
        //private int GetIdFromIvo(string ivo)
        //{
        //    var handler = new JwtSecurityTokenHandler();
        //    var jwtToken = handler.ReadJwtToken(ivo);

        //    var claims = jwtToken.Claims;
        //    var id = claims.FirstOrDefault(x => x.Type == "studentId").Value;


        //    int.TryParse(id, out int returnId);
        //    return returnId;
        //}
        //private UserAuthData UserAuthData => GetUserData()

        //private readonly CreateStudentRequestHandler _createHandler;

        //public StudentsController(CreateStudentRequestHandler createHandler)
        //{
        //    _createHandler = createHandler;
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAll(
        //    [FromServices] IStudentRepository studentRepository
        //    )
        //{
        //    var requestHandler = new GetAllStudentsRequestHandler(studentRepository);
        //    var result = await requestHandler.ProcessAuthorizedRequestAsync(new GetAllRequest());

        //    return result.ToActionResult(this);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var requestHandler = new GetStudentByIdRequestHandler(/* ako treba */);
        //    var result = await requestHandler.ProcessAuthorizedRequestAsync(new GetByIdRequest(id));

        //    return result.ToActionResult(this);
        //} OVO JOS NEMAMO IMAMO HANDLER SAMO ZA DODATI

        [Authorize]
        [HttpGet("me")] //maknuo sam rutu id, izvalci se iz headera
        public async Task<ActionResult> GetById(
                [FromServices] IStudentRepository studentRepository,
                [FromServices] ICurrentUserService currentUser
            )
        {

            var request = new GetByIdRequest(); //iz headera tokena
            var handler = new GetByIdRequestHandler(studentRepository, currentUser);
            var result = await handler.ProcessAuthorizedRequestAsync(request);

            return result.ToActionResult(this);
        }

        // POST IMAMO
        [Authorize(Roles = "Admin")]
        [HttpPost] //ova metoda doli reagira samo na POST
        public async Task<ActionResult> Post( //actionresult je tip povratne vrijednosti, ok, bad, vidis u ext
            [FromServices] IStudentRepository studentRepository, //.net ubacuje instacu iz addscoped, iz DI kontejnera
            [FromBody] CreateStudentRequest request, //uzmi iz tijela http zahtjeva { "firstName": "Jure", "lastName": "Horvat" }, .NET TO AUTOMATSKI DESERIALIZIRA U OBJEKT CreateStudentRequest DTO taj
             [FromServices] ICacheService<GetAllResponse<StudentDto>> cacheService
            ) //from body iz {} izvuce
        {
            var requestHandler = new CreateStudentRequestHandler(studentRepository, cacheService);
            var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }

        [Authorize]
        [HttpPut("update")] //ova metoda doli reagira samo na POST
        public async Task<ActionResult> Update(
            [FromServices] IStudentRepository studentRepository,
            [FromBody] UpdateStudentRequest request,
            [FromServices] ICurrentUserService currentUser,
                 [FromServices] ICacheService<GetAllResponse<StudentDto>> cacheService
        )
        {

            var requestHandler = new UpdateStudentRequestHandler(studentRepository, currentUser, cacheService);
            var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")] //ova metoda doli reagira samo na POST
        public async Task<ActionResult> Delete(
          [FromRoute] int id,
          [FromServices] IStudentRepository studentRepository,
                           [FromServices] ICacheService<GetAllResponse<StudentDto>> cacheService

      )
        {
            var request = new DeleteStudentRequest { Id = id };
            var requestHandler = new DeleteStudentRequestHandler(studentRepository, cacheService);
            var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get(
              [FromServices] IStudentRepository studentRepository,
             //  [FromBody] GetAllStudentsRequestHandler request  //u bodyu nemam nista, ali handle prima ovaj request, moram ga imati
             [FromServices] ICurrentUserService currentUser,
            [FromServices] ICacheService<GetAllResponse<StudentDto>> cacheService
            )
        {
            var request = new GetAllStudentsRequest(); //instaciramo nista praznu klasu
            var requestHandler = new GetAllStudentsRequestHandler(studentRepository, currentUser, cacheService);
            var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }


        [Authorize]
        [HttpGet("{id}/active-borrows")]
        public async Task<ActionResult> GetActiveBorrowsForStudent(
            [FromRoute] int id,
            [FromServices] IBorrowUnitOfWork unitOfWork
        )
        {
            var handler = new GetActiveBorrowsForStudentRequestHandler(unitOfWork);
            var result = await handler.ProcessAuthorizedRequestAsync(new GetActiveBorrowsRequest { Id = id });
            return result.ToActionResult(this);
        }

    }
}