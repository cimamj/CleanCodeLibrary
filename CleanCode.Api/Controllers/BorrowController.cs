using CleanCode.Api.Common;
using CleanCodeLibrary.Application.Borrows.Borrow;
using CleanCodeLibrary.Domain.Persistance.Common;
using Microsoft.AspNetCore.Mvc;

namespace CleanCode.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowsController : ControllerBase
    {
       
        [HttpPost]
        public async Task<ActionResult> BorrowBook(
            [FromServices] IUnitOfWork unitOfWork,
            [FromBody] BorrowBookRequest request)
        {
            var handler = new BorrowBookRequestHandler(unitOfWork);
            var result = await handler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }

        [HttpPut("{id}")] 
        public async Task<ActionResult> ReturnBook(
            [FromRoute] int id,
            [FromServices] IUnitOfWork unitOfWork,
            [FromBody] ReturnBookRequest request)
        {
            request.BorrowId = id;
            var handler = new ReturnBookRequestHandler(unitOfWork);
            var result = await handler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }
    }
}