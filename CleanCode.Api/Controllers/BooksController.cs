using CleanCode.Api.Common;
using CleanCodeLibrary.Application.Books.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanCode.Api.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly CreateBookRequestHandler _createHandler;
        private readonly UpdateBookRequestHandler _updateHandler;
        private readonly DeleteBookRequestHandler _deleteHandler;
        private readonly GetAllBooksRequestHandler _getAllHandler;
        private readonly GetByIdRequestHandler _getByIdHandler;
        private readonly GetUsedGenresRequestHandler _getUsedGenresHandler;
        private readonly GetTopBooksRequestHandler _getTopBooksHandler;

        public BooksController(
            CreateBookRequestHandler createHandler,
            UpdateBookRequestHandler updateHandler,
            DeleteBookRequestHandler deleteHandler,
            GetAllBooksRequestHandler getAllHandler,
            GetByIdRequestHandler getByIdHandler,
            GetUsedGenresRequestHandler getUsedGenresHandler,
            GetTopBooksRequestHandler getTopBooksHandler)
        {
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _deleteHandler = deleteHandler;
            _getAllHandler = getAllHandler;
            _getByIdHandler = getByIdHandler;
            _getUsedGenresHandler = getUsedGenresHandler;
            _getTopBooksHandler = getTopBooksHandler;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateBookRequest request)
        {
            var result = await _createHandler.ProcessAuthorizedRequestAsync(request);

            return result.ToActionResult(this);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateBookRequest request)
        {
            request.Id = id;

            var result = await _updateHandler.ProcessAuthorizedRequestAsync(request);

            return result.ToActionResult(this);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var result = await _deleteHandler.ProcessAuthorizedRequestAsync(new DeleteBookRequest { Id = id });

            return result.ToActionResult(this);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _getAllHandler.ProcessAuthorizedRequestAsync(new GetAllBooksRequest { PageNumber = pageNumber, PageSize = pageSize });

            return result.ToActionResult(this);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var result = await _getByIdHandler.ProcessAuthorizedRequestAsync(new GetByIdRequest { Id = id });

            return result.ToActionResult(this);
        }

        [Authorize]
        [HttpGet("used-genres")]
        public async Task<ActionResult> GetUsedGenres()
        {
            var result = await _getUsedGenresHandler.ProcessAuthorizedRequestAsync(new GetUsedGenresRequest());

            return result.ToActionResult(this);
        }

        [Authorize]
        [HttpGet("top")]
        public async Task<ActionResult> GetTopBooks()
        {
            var result = await _getTopBooksHandler.ProcessAuthorizedRequestAsync(new GetTopBooksRequest());

            return result.ToActionResult(this);
        }
    }
}
