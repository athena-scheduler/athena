using SimpleMigrations;
namespace Athena.Data.Migrations
{
    [Migration(20180215110432, "Identity")]
    public class Identity : ScriptMigration
    {
        public Identity() : base("20180215110432_Identity.sql")
        {
        }
    }
}
