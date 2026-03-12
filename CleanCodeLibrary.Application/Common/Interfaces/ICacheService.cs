

namespace CleanCodeLibrary.Application.Common.Interfaces
{
    public interface ICacheService<T> where T : class
    {
        Task<T> GetOrSetAsync(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
        void Invalidate(string key);
    }
}
