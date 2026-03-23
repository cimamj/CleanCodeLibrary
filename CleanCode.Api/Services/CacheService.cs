using CleanCodeLibrary.Application.Common.Interfaces;
using CleanCodeLibrary.Domain.Common.Model;
using CleanCodeLibrary.Domain.DTOs.Books;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace CleanCode.Api.Services
{
    public class CacheService<T> : ICacheService<T> where T : class //bitno za vidjeti je li null
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public Task<T?> GetOrSetAsync(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            //if (_cache.TryGetValue(key, out T cached))
            //    return cached;

            //var result = await factory();
            //ode ubacis    if (result is null) return result; ne kashiraj
            //_cache.Set(key, result, TimeSpan.FromMinutes(5));
            //return result;

           
           
            return _cache.GetOrCreateAsync(key, async entry => //provjeri postoji li key, ako ne, ZAKLJUCA po keyu (svi naredni zahtjevi za isti key stanu i cekaju da ovi setira iz factory) i provjerava cache inace tuce factory
            {
                entry.AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5);
                return await factory();
            });
        }

        public void Invalidate(string key)
        {
            _cache.Remove(key);
        }
    }
}
