using CleanCodeLibrary.Application.Common.CacheKeys;
using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.Common.Validation;
using CleanCodeLibrary.Domain.DTOs.Books;
using CleanCodeLibrary.Domain.Persistance.Borrows;
using CleanCodeLibrary.Domain.Persistance.Common;
using Borrow = CleanCodeLibrary.Domain.Entities.Borrows.Borrow;

public class ReturnBookRequest
{
    public int BorrowId { get; set; }
}

public class ReturnBookRequestHandler
    : RequestHandler<ReturnBookRequest, SuccessResponse>
{
    private readonly IBorrowUnitOfWork _unitOfWork;
    private readonly ICacheService<GetAllResponse<BookDto>> _cache; //bilo koji tip, jer pozivas invalidate metodu koja ne ovisi o generickom tipu T koji moze bit npr GetAllResponse<BookDto> ili GetTitles<BookTitles> nebitno
    public ReturnBookRequestHandler(IBorrowUnitOfWork unitOfWork, ICacheService<GetAllResponse<BookDto>> cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    protected override async Task<Result<SuccessResponse>> HandleRequest(
        ReturnBookRequest request,
        Result<SuccessResponse> result)
    {
        var item = new Borrow
        {
            Id = request.BorrowId
        };

        //var existingBorrow = await _unitOfWork.BorrowRepository.GetById(request.BorrowId);
        //if (existingBorrow == null)
        //{
        //    result.AddError(new ValidationResultItem
        //    {
        //        Code = "Borrow.NotFound",
        //        Message = "Posudba ne postoji",
        //        ValidationSeverity = ValidationSeverity.Error,
        //        ValidationType = ValidationType.NotFound
        //    });
        //    return result;
        //}

        //on je ode sve napravia u domainu, a npr update je drukcije zasto?
        var domainResult = await item.Return(_unitOfWork);
        result.SetValidationResult(domainResult.ValidationResult);
        if (result.HasError)
            return result;

        await _unitOfWork.SaveAsync();

        _cache.Invalidate(Keys.AllBooks);
        _cache.Invalidate(Keys.TopBooks10);

        result.SetResult(new SuccessResponse { IsSuccess = true });
        return result;
    }

    protected override Task<bool> IsAuthorized() => Task.FromResult(true);
}