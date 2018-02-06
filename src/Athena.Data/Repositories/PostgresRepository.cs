using System;
using System.Data;

namespace Athena.Data.Repositories
{
    public class PostgresRepository : IDisposable
    {
        protected readonly IDbConnection _db;

        public PostgresRepository(IDbConnection db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}