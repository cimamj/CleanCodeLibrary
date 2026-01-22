using CleanCodeLibrary.Application.Common.Model;
using CleanCodeLibrary.Domain.Persistance.Common;

public class ReturnBookRequest
{
    public int BorrowId { get; set; }
}

public class ReturnBookRequestHandler
    : RequestHandler<ReturnBookRequest, SuccessResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public ReturnBookRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task<Result<SuccessResponse>> HandleRequest(
        ReturnBookRequest request,
        Result<SuccessResponse> result)
    {
        var domainResult = await CleanCodeLibrary.Domain.Entities.Borrows.Borrow
            .Return(_unitOfWork, request.BorrowId);

        result.SetValidationResult(domainResult.ValidationResult);

        if (result.HasError)
        {
            return result;
        }

        await _unitOfWork.SaveAsync();

        result.SetResult(new SuccessResponse { IsSuccess = true });
        return result;
    }

    protected override Task<bool> IsAuthorized() => Task.FromResult(true);
}