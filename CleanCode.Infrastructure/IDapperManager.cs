using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCode.Infrastructure
{
    public interface IDapperManager
    {
        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null);     // SELECT više redaka
        Task<T?> QuerySingleAsync<T>(string sql, object? param = null);            // SELECT jedan redak
        Task ExecuteAsync(string sql, object? param = null);                       // INSERT/UPDATE/DELETE
    }
}
