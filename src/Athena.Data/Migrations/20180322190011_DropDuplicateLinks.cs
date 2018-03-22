using SimpleMigrations;
namespace Athena.Data.Migrations
{
    [Migration(20180322190011, "DropDuplicateLinks")]
    public class DropDuplicateLinks : ScriptMigration
    {
        public DropDuplicateLinks() : base("20180322190011_DropDuplicateLinks.sql")
        {
        }
    }
}
