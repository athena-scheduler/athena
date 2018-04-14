using SimpleMigrations;
namespace Athena.Data.Migrations
{
    [Migration(20180414114233, "IdentityTrackSecurityStamps")]
    public class IdentityTrackSecurityStamps : ScriptMigration
    {
        public IdentityTrackSecurityStamps() : base("20180414114233_IdentityTrackSecurityStamps.sql")
        {
        }
    }
}
