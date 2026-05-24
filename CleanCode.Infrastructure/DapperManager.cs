using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCode.Infrastructure
{
    public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.Value = value.ToDateTime(TimeOnly.MinValue);
        }

        public override DateOnly Parse(object value)
        {
            return DateOnly.FromDateTime((DateTime)value);
        }
    }

    internal sealed class DapperManager : IDapperManager
    {
        private readonly string _connectionString;

        public DapperManager(string connectionString)
        {
            _connectionString = connectionString;

            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public async Task ExecuteAsync(string sql, object? param = null)
        {
            using var connection = CreateConnection();

            await connection.OpenAsync();

            await connection.ExecuteAsync(sql, param);
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null)
        {
            using var connection = CreateConnection();

            await connection.OpenAsync();

            var result = await connection.QueryAsync<T>(sql, param);

            return result.AsList();
        }

        public async Task<T?> QuerySingleAsync<T>(string sql, object? param = null)
        {
            using var connection = CreateConnection();

            await connection.OpenAsync();

            return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
        }
    }
}
