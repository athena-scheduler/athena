using SimpleMigrations;
namespace Athena.Data.Migrations
{
    [Migration(20180412163557, "EnsureConcurrentPrereqsAreNotRegularPrereqs")]
    public class EnsureConcurrentPrereqsAreNotRegularPrereqs : ScriptMigration
    {
        public EnsureConcurrentPrereqsAreNotRegularPrereqs() : base("20180412163557_EnsureConcurrentPrereqsAreNotRegularPrereqs.sql")
        {
        }
    }
}
