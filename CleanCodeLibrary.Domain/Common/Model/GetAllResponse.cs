namespace CleanCodeLibrary.Domain.Common.Model
{
    public class GetAllResponse<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> Values
        {
            get; init;
        }
    }
}
