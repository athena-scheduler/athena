using SimpleMigrations;
namespace Athena.Data.Migrations
{
    [Migration(20180414190650, "ExternalMeetings")]
    public class ExternalMeetings : ScriptMigration
    {
        public ExternalMeetings() : base("20180414190650_ExternalMeetings.sql")
        {
        }
    }
}
