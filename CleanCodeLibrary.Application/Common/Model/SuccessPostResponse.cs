
namespace CleanCodeLibrary.Application.Common.Model
{
    public class SuccessPostResponse
    {
        public int? Id { get; init; }
        public SuccessPostResponse(int? id) 
        {
            Id = id;
        }
        public SuccessPostResponse() 
        {
        
        }
    }
}
