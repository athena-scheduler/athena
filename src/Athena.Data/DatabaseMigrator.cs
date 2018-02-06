using Npgsql;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;

namespace Athena.Data
{
    public class DatabaseMigrator
    {
        private readonly string _connectionString;

        public DatabaseMigrator(string connectionString) => _connectionString = connectionString;

        public void Migrate()
        {
            using (var db = new NpgsqlConnection(_connectionString))
            {
                var provider = new PostgresqlDatabaseProvider(db);
                var migrator = new SimpleMigrator(typeof(DatabaseMigrator).Assembly, provider);

                migrator.Load();
                migrator.MigrateToLatest();
            }
        }
    }
}