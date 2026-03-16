using CleanCode.Api.Common;
using CleanCodeLibrary.Application.Borrows.Borrow;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Common;
using Microsoft.AspNetCore.Mvc;

namespace CleanCode.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowsController : ControllerBase
    {
       
        //[HttpPost]
        //public async Task<ActionResult> BorrowBook(
        //    [FromServices] IUnitOfWork unitOfWork,
        //    [FromBody] BorrowBookRequest request)
        //{
        //    var handler = new BorrowBookRequestHandler(unitOfWork);
        //    var result = await handler.ProcessAuthorizedRequestAsync(request);
        //    return result.ToActionResult(this);
        //}

        [HttpPost] 
        public async Task<ActionResult> BorrowBook(
        [FromServices] IBorrowUnitOfWork unitOfWork,
        [FromBody] CreateBorrowAndUpdateBookAmountRequest request,
        [FromServices] ICacheService<GetAllResponse<BookDto>> cacheService
        )
        {
            var handler = new CreateBorrowAndUpdateBookAmountRequestHandler(unitOfWork, cacheService);
            var result = await handler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }

        [HttpPut("{id}/return")] 
        public async Task<ActionResult> ReturnBook(
            [FromRoute] int id,
            [FromServices] IBorrowUnitOfWork unitOfWork,
             [FromServices] ICacheService<GetAllResponse<BookDto>> cacheService
           )
        {
            var handler = new ReturnBookRequestHandler(unitOfWork, cacheService);
            var result = await handler.ProcessAuthorizedRequestAsync(new ReturnBookRequest { BorrowId = id });
            return result.ToActionResult(this);
        }
    }
}