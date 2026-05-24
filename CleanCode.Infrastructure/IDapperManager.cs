using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCode.Infrastructure
{
    public interface IDapperManager
    {
        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null);

        Task<T?> QuerySingleAsync<T>(string sql, object? param = null);

        Task ExecuteAsync(string sql, object? param = null);
    }
}
