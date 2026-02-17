using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Model; //problem sto sad nezna koji result koristiti, prominit cu ime u domainu resultDomain
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Persistance.Books;

namespace CleanCodeLibrary.Application.Books.Book
{
    public class GetAllBooksRequest { }


    public class GetAllBooksRequestHandler //pitaj ivu koji kurac
        : RequestHandler<GetAllBooksRequest, GetAllResponse<BookDto>>
    {
        private readonly IBookRepository _bookRepository;

        public GetAllBooksRequestHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        //a npr kad mapiran u novu klasu radi

        protected override async Task<Result<GetAllResponse<BookDto>>> HandleRequest(
            GetAllBooksRequest request,
            Result<GetAllResponse<BookDto>> result)
        {
            var books = await _bookRepository.GetAllBookDtos();
            if(books.Values.Count() == 0) //validan rezultat, samo warning, ili !Values.Any()
            {
                result.AddWarning(new ValidationResultItem
                {
                    Message = "Nema knjiga u bazi",
                    ValidationSeverity = ValidationSeverity.Warning,
                });
                //return result; OVAKO RESULT NECE IMATI VALUE, UC CE U RESPONSEEXT I VRATIT CE 404, TO NE ZELIMO
            }

            result.SetResult(books);
            return result;
        }

        protected override Task<bool> IsAuthorized() => Task.FromResult(true);
    }
}
//GLUPO OPET MAPIRATI NEMA SMISLA NOVI DTO ISPADA, ali konflikt radi result odavde  i iz domaina,
//GetById, GetAll → Repository direktno (nema validacije) ali getbyid mapira u repoziotriju jel to ok? vraca mapirani book, a ode mapiranje isto ali jos jednom mapiran u drugu klasu radi konflikta
//Create, Update, Delete → Domain (ima validaciju)