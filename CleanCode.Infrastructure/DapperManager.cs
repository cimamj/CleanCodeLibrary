using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCode.Infrastructure
{
    internal sealed class DapperManager : IDapperManager
    {
        private readonly string _connectionString;

        public DapperManager(string connectionString)
        {
            _connectionString = connectionString;
        }
    //konekcija na bazu

        private NpgsqlConnection CreateConnection() //dapper ovako otvara konekciju 
        {
            return new NpgsqlConnection(_connectionString); //ugradenoj klasi saljemo string po kojoj se on spaja na bazu, kako se EF spaja, pa preko dbcontexta + connectionstring u app.json, konekcijama upravlja sam
        }
        public async Task ExecuteAsync(string sql, object? param = null)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            await connection.ExecuteAsync(sql, param);
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null) //zelimo dohvatit preko dappera, konekcija otvorena saljes cisti sql, ode dohvacamo vjv listu 
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            var result = await connection.QueryAsync<T>(sql, param);
            return result.AsList();
        }

        public async Task<T?> QuerySingleAsync<T>(string sql, object? param = null) //ode dohvacamo jednoga po keyu unikatnome
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            return await connection.QuerySingleOrDefaultAsync<T>(sql, param); 
        } //objasni sve ovo, na primjeru log

    }
}
