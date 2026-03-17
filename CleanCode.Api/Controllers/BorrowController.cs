using CleanCode.Api.Common;
using CleanCodeLibrary.Application.Borrows.Borrow;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanCode.Api.Controllers
{
    [Route("api/borrows")]
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
        [Authorize]
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
        [Authorize]
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

        [Authorize]
        [HttpGet("{studentId}/borrow-statistics")]
        public async Task<ActionResult> GetBorrowStatistics(
    [FromRoute] int studentId,
    [FromServices] IBorrowRepository borrowRepository)
        {
            var handler = new GetBorrowStatisticsRequestHandler(borrowRepository);
            var request = new GetBorrowStatisticsRequest(studentId);
            var result = await handler.ProcessAuthorizedRequestAsync(request);
            return result.ToActionResult(this);
        }
    }
}