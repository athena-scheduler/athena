using SimpleMigrations;
namespace Athena.Data.Migrations
{
    [Migration(20180329115355, "RefactorOfferings")]
    public class RefactorOfferings : ScriptMigration
    {
        public RefactorOfferings() : base("20180329115355_RefactorOfferings.sql")
        {
        }
    }
}
