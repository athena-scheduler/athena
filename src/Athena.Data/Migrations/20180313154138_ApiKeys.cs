using SimpleMigrations;
namespace Athena.Data.Migrations
{
    [Migration(20180313154138, "ApiKeys")]
    public class ApiKeys : ScriptMigration
    {
        public ApiKeys() : base("20180313154138_ApiKeys.sql")
        {
        }
    }
}
