using Microsoft.AspNetCore.Mvc;
using CleanCodeLibrary.Application.Students.Student;
using CleanCodeLibrary.Application.Common.Model;
using CleanCode.Api.Common;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Students;

namespace CleanCode.Api.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
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


        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(
                [FromRoute] int id,
                [FromServices] IStudentRepository studentRepository
            )
        {
            var request = new GetByIdRequest { Id = id }; //ne triba iz bodya?
            var handler = new GetByIdRequestHandler(studentRepository);
            var result = await handler.ProcessAuthorizedRequestAsync(request);

            return result.ToActionResult(this);
        }

        // POST IMAMO
        [HttpPost] //ova metoda doli reagira samo na POST
        public async Task<ActionResult> Post( //actionresult je tip povratne vrijednosti, ok, bad, vidis u ext
            [FromServices] IStudentRepository studentRepository, //.net ubacuje instacu iz addscoped, iz DI kontejnera
            [FromBody] CreateStudentRequest request //uzmi iz tijela http zahtjeva { "firstName": "Jure", "lastName": "Horvat" }, .NET TO AUTOMATSKI DESERIALIZIRA U OBJEKT CreateStudentRequest DTO taj
            ) //from body iz {} izvuce
        {
            var requestHandler = new CreateStudentRequestHandler(studentRepository);
            var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }

        [HttpPut("{id}")] //ova metoda doli reagira samo na POST
        public async Task<ActionResult> Update(
            [FromRoute] int id,
            [FromServices] IStudentRepository studentRepository, 
            [FromBody] UpdateStudentRequest request 
        ) 
        {
            // Ako trebaš ID iz rute provjeriti s requestom – možeš dodati validaciju ???? ne kuzim
            request.Id = id; //dolazi iz routt ne body
            var requestHandler = new UpdateStudentRequestHandler(studentRepository);
            var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id}")] //ova metoda doli reagira samo na POST
        public async Task<ActionResult> Delete(
           [FromRoute] int id,
           [FromServices] IStudentRepository studentRepository
       )
        {
            var request = new DeleteStudentRequest { Id = id };
            var requestHandler = new DeleteStudentRequestHandler(studentRepository);
            var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }


        [HttpGet]
        public async Task<ActionResult> Get(
              [FromServices] IStudentRepository studentRepository
            //  [FromBody] GetAllStudentsRequestHandler request  //u bodyu nemam nista, ali handle prima ovaj request, moram ga imati
            )
        {
            var request = new GetAllStudentsRequest(); //instaciramo nista praznu klasu
            var requestHandler = new GetAllStudentsRequestHandler(studentRepository);
            var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }



    }
}