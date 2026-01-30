

namespace CleanCodeLibrary.Application.Common.Model
{
    public class GetAllResponse<TEntity> where TEntity : class //<> s ovim se barata u ovoj klasi, npr ima polje tipa <> 
    {
        public IEnumerable<TEntity> Values { get; init; }//ne tribaju nam funkcije od liste, stedimo memoriju 
    }
}
