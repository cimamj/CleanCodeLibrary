
namespace CleanCodeLibrary.Application.Common.Model
{
    public class SuccessDeleteResponse
    {
        public int? Id { get; init; }
        public SuccessDeleteResponse(int? id)
        {
            Id = id;
        }
        public SuccessDeleteResponse()
        {

        }
    }
}
