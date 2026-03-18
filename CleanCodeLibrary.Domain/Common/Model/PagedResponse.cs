

namespace CleanCodeLibrary.Domain.Common.Model
{
    public class PagedResponse<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> Values { get; init; }
        public int TotalCount { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
