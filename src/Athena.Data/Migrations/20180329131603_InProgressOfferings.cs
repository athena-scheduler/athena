using SimpleMigrations;
namespace Athena.Data.Migrations
{
    [Migration(20180329131603, "InProgressOfferings")]
    public class InProgressOfferings : ScriptMigration
    {
        public InProgressOfferings() : base("20180329131603_InProgressOfferings.sql")
        {
        }
    }
}
