

namespace CleanCodeLibrary.Domain.Common.Model
{
    public class GetByIdResponse
    {
        public int Id { get; init; }

        public GetByIdResponse(int id) { Id = id; }
        public GetByIdResponse() { }
    }
    public class GetByIdResponse<T>
    {
        public T Id { get; init; }
        public GetByIdResponse(T id) { Id = id; }
        public GetByIdResponse() { }
    }
}