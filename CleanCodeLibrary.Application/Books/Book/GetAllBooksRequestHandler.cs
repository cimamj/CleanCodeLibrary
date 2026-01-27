
using CleanCodeLibrary.Application.Common.Model;
//using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Entities.Books;
using CleanCodeLibrary.Domain.Persistance.Books;
using CleanCodeLibrary.Domain.DTOs.Books;
using System.Xml.XPath;

namespace CleanCodeLibrary.Application.Books.Book
{
    public class GetAllBooksRequest
    {

    }


    public class AllBooksResponse
    {
        public IEnumerable<BookDto> _books { get; set; }
        public AllBooksResponse(IEnumerable<BookDto> Books) { _books = Books; }
    }

        public class GetAllBooksRequestHandler : RequestHandler<GetAllBooksRequest, AllBooksResponse>
    {
        public IBookRepository _bookRepository { get; set; }
        public GetAllBooksRequestHandler(IBookRepository bookRepository) //ono sto vracamo obavezno u value u result
        {
            _bookRepository = bookRepository;
        }

        protected override async Task<Result<AllBooksResponse>> HandleRequest(GetAllBooksRequest request, Result<AllBooksResponse> result)
        {
            //var domainResult = await CleanCodeLibrary.Domain.Entities.Books.Book.GetAllBooksAsync(_bookRepository); //kad stavim samo static tu metodu nemogu , mora cila klasa za metodu korsitit?
            //result.SetValidationResult(domainResult.ValidationResult);

            //if (result.HasError)
            //{
            //    return result;
            //}
            ////inace je puna lista
            //var booksDto = domainResult.Value.Values.Select(b => new BookDto
            //{
            //    Author = b.Author,
            //    Title = b.Title,
            //    Isbn = b.Isbn,
            //    Year = b.Year,
            //    Genre = b.Genre
            //});

            ////ne triba savechangess

            //result.SetResult(new AllBooksResponse(booksDto));
            //return result;

            //direktno priko repoziotija 
            //var books = await _bookRepository.Get();
            ////ide li neka provjera je li pranzo prazna lista je validan rezultat?

            //var dtos = books.Values.Select(b => new BookDto
            //{
            //    Title = b.Title,
            //    Author = b.Author,
            //    Isbn = b.Isbn,
            //    Year = b.Year,
            //    Genre = b.Genre,
            //});

            //result.SetResult(new AllBooksResponse(dtos));
            //return result;

            //zasto ne omgu korisitti getallresposne iz domaina kojeg svakako dobijan

            //ili ako cu koristiti GetAllBookDtos 
            var books = await _bookRepository.GetAllBookDtos(); //books je objekt sa values
            result.SetResult(new AllBooksResponse(books.Values)); //jel potriba opet nova klasa
            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);

    }
}
