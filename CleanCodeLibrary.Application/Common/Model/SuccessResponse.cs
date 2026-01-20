
namespace CleanCodeLibrary.Application.Common.Model
{
    public class SuccessResponse
    {
        public bool IsSuccess {  get; init; }
        public  SuccessResponse() { }   
        public SuccessResponse(bool isSuccess) { IsSuccess = isSuccess; }
    }
}
