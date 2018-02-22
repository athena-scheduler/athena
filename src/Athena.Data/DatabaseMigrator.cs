using Npgsql;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;

namespace Athena.Data
{
    public class DatabaseMigrator
    {
        private readonly string _connectionString;

        public DatabaseMigrator(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// The database needs migrated to do anything so this is a good place for global database init stuff
        /// </summary>
        static DatabaseMigrator()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
        
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