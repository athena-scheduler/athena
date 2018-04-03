using System;
using System.Data;
using System.Transactions;
using Athena.Data.Extensions;
using AutoFixture;
using Npgsql;

namespace Athena.Data.Tests
{
    public class DataTest : IDisposable
    {
        public const string DEFAULT_CONNECTION_STRING = "Server=localhost;User ID=postgres;Database=postgres";

        private static readonly string ConnectionString = Environment.GetEnvironmentVariable("ATHENA_DATA_TESTS_CON")
                                                          ?? DEFAULT_CONNECTION_STRING;

        protected readonly IDbConnection _db;
        protected readonly Fixture _fixture;

        private readonly TransactionScope _scope;

        static DataTest() => new DatabaseMigrator(ConnectionString).Migrate();

        protected DataTest()
        {
            _db = new NpgsqlConnection(ConnectionString);
            if (_db.State != ConnectionState.Open)
            {
                _db.Open();
            }

            _fixture = new Fixture();
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
