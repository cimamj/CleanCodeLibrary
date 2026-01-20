
namespace CleanCodeLibrary.Domain.Common.Model
{
    public class GetAllResponse<TEntity> //<> s ovim se barata u ovoj klasi, npr ima polje tipa <> 
    {
        public IEnumerable<TEntity> Values { get; init; }//ne tribaju nam funkcije od liste, stedimo memoriju 
    }
}
