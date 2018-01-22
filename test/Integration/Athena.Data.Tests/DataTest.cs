using System;
using System.Data;
using System.Transactions;
using Npgsql;

namespace Athena.Data.Tests
{
    public class DataTest : IDisposable
    {
        public const string DEFAULT_CONNECTION_STRING = "Server=localhost;User ID=postgres;Database=postgres";

        private static readonly string ConnectionString = Environment.GetEnvironmentVariable("ATHENA_DATA_TESTS_CON")
                                                          ?? DEFAULT_CONNECTION_STRING;

        private readonly IDbConnection _db;

        private readonly TransactionScope _scope;

        static DataTest() => new DatabaseMigrator(ConnectionString).Migrate();
        
        public DataTest()
        {
            _db = new NpgsqlConnection(ConnectionString);
            if (_db.State != ConnectionState.Open)
            {
                _db.Open();
            }

            _scope = _db.CreateAsyncTransactionScope();
        }
        
        public void Dispose()
        {
            // By not calling Complete() on the transaction scope, we roll it back
            _scope.Dispose();
            _db.Dispose();
        }
    }
}
