using CleanCode.Api.Common;
using CleanCodeLibrary.Application.Students.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanCode.Api.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly GetByIdRequestHandler _getByIdHandler;
        private readonly CreateStudentRequestHandler _createHandler;
        private readonly UpdateStudentRequestHandler _updateHandler;
        private readonly DeleteStudentRequestHandler _deleteHandler;
        private readonly GetAllStudentsRequestHandler _getAllHandler;
        private readonly GetActiveBorrowsForStudentRequestHandler _getActiveBorrowsHandler;

        public StudentsController(
            GetByIdRequestHandler getByIdHandler,
            CreateStudentRequestHandler createHandler,
            UpdateStudentRequestHandler updateHandler,
            DeleteStudentRequestHandler deleteHandler,
            GetAllStudentsRequestHandler getAllHandler,
            GetActiveBorrowsForStudentRequestHandler getActiveBorrowsHandler)
        {
            _getByIdHandler = getByIdHandler;
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _deleteHandler = deleteHandler;
            _getAllHandler = getAllHandler;
            _getActiveBorrowsHandler = getActiveBorrowsHandler;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult> GetById()
        {
            var result = await _getByIdHandler.ProcessAuthorizedRequestAsync(new GetByIdRequest());

            return result.ToActionResult(this);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateStudentRequest request)
        {
            var result = await _createHandler.ProcessAuthorizedRequestAsync(request);

            return result.ToActionResult(this);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody] UpdateStudentRequest request)
        {
            var result = await _updateHandler.ProcessAuthorizedRequestAsync(request);

            return result.ToActionResult(this);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var result = await _deleteHandler.ProcessAuthorizedRequestAsync(new DeleteStudentRequest { Id = id });

            return result.ToActionResult(this);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _getAllHandler.ProcessAuthorizedRequestAsync(new GetAllStudentsRequest());

            return result.ToActionResult(this);
        }

        [Authorize]
        [HttpGet("{id}/active-borrows")]
        public async Task<ActionResult> GetActiveBorrowsForStudent([FromRoute] int id)
        {
            var result = await _getActiveBorrowsHandler.ProcessAuthorizedRequestAsync(new GetActiveBorrowsRequest { Id = id });

            return result.ToActionResult(this);
        }
    }
}
