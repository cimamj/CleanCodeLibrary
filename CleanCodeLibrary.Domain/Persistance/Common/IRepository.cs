using CleanCodeLibrary.Domain.Common.Model;
namespace CleanCodeLibrary.Domain.Persistance.Common
{
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        Task InsertAsync(TEntity entity);

        Task<GetAllResponse<TEntity>> Get();

        void Update(TEntity entity);

        Task<bool> DeleteAsync(TId id);

        Task SaveAsync();
    }
}
