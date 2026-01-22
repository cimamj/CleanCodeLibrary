using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeLibrary.Application.Books.Book
{
    public class GetByIdRequest
    {
        public int Id { get; set; }
    }

    public class BookResponse
    {
        public BookDto _book { get; set; } //ovaj tip je na istoj razini u getallbooks

        public BookResponse(BookDto Book) 
        {
            _book = Book;   
        }

        public BookResponse() { }
    }


    public class GetByIdRequestHandler : RequestHandler<GetByIdRequest, BookResponse>
    {
        public IBookRepository _bookRepository { get; set; }
        public GetByIdRequestHandler(IBookRepository bookRepository) 
        {
            _bookRepository = bookRepository;
        }

        protected async override Task<Result<BookResponse>> HandleRequest(GetByIdRequest request, Result<BookResponse> result)
        {
            var bookFromDomain = await CleanCodeLibrary.Domain.Entities.Books.Book.GetByIdDomainAsync(_bookRepository, request.Id);
            result.SetValidationResult(bookFromDomain.ValidationResult);

            if(result.HasError)
            {
                return result;
            }

            var bookDto = new BookDto
            {
                Author = bookFromDomain.Value.Author,
                Title = bookFromDomain.Value.Title,
                Isbn = bookFromDomain.Value.Isbn,
                Year = bookFromDomain.Value.Year,
                Genre = bookFromDomain.Value.Genre
            };

            result.SetResult(new BookResponse(bookDto));
            return result;

        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);

    }
}
