using CleanCode.Api.Common;
using CleanCodeLibrary.Application.Borrows.Borrow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanCode.Api.Controllers
{
    [Route("api/borrows")]
    [ApiController]
    public class BorrowsController : ControllerBase
    {
        private readonly CreateBorrowAndUpdateBookAmountRequestHandler _createBorrowHandler;
        private readonly ReturnBookRequestHandler _returnBookHandler;
        private readonly GetBorrowStatisticsRequestHandler _statisticsHandler;

        public BorrowsController(
            CreateBorrowAndUpdateBookAmountRequestHandler createBorrowHandler,
            ReturnBookRequestHandler returnBookHandler,
            GetBorrowStatisticsRequestHandler statisticsHandler)
        {
            _createBorrowHandler = createBorrowHandler;
            _returnBookHandler = returnBookHandler;
            _statisticsHandler = statisticsHandler;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> BorrowBook([FromBody] CreateBorrowAndUpdateBookAmountRequest request)
        {
            var result = await _createBorrowHandler.ProcessAuthorizedRequestAsync(request);

            return result.ToActionResult(this);
        }

        [Authorize]
        [HttpPut("{id}/return")]
        public async Task<ActionResult> ReturnBook([FromRoute] int id)
        {
            var result = await _returnBookHandler.ProcessAuthorizedRequestAsync(new ReturnBookRequest { BorrowId = id });

            return result.ToActionResult(this);
        }

        [Authorize]
        [HttpGet("{studentId}/borrow-statistics")]
        public async Task<ActionResult> GetBorrowStatistics([FromRoute] int studentId)
        {
            var result = await _statisticsHandler.ProcessAuthorizedRequestAsync(new GetBorrowStatisticsRequest(studentId));

            return result.ToActionResult(this);
        }
    }
}
