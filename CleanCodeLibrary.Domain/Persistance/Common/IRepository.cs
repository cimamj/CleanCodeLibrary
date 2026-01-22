using CleanCodeLibrary.Domain.Common.Model;


namespace CleanCodeLibrary.Domain.Persistance.Common
{
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        Task InsertAsync(TEntity entity); //rjeseno domain create
        Task<GetAllResponse<TEntity>> Get(); //vraca objekt koji ima listu TEntitya (entiteta)
        void Update(TEntity entity); //prominija sam iz task u void, tj iz updateasync u update

        Task<bool> DeleteAsync(TId id);
        Task SaveAsync(); 
        //ode nemam updateasync, borrow ne uzima istog
    }
}
