using SimpleMigrations;
namespace Athena.Data.Migrations
{
    [Migration(20180121141736, "InitialState")]
    public class InitialState : ScriptMigration
    {
        public InitialState() : base("20180121141736_InitialState.sql")
        {
        }
    }
}
