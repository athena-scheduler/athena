using SimpleMigrations;
namespace Athena.Data.Migrations
{
    [Migration(20180329144035, "FixOfferingMeetingRelationship")]
    public class FixOfferingMeetingRelationship : ScriptMigration
    {
        public FixOfferingMeetingRelationship() : base("20180329144035_FixOfferingMeetingRelationship.sql")
        {
        }
    }
}
