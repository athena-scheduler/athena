using System;
using System.IO;
using SimpleMigrations;

namespace Athena.Data.Migrations
{
    public abstract class ScriptMigration : Migration
    {
        private readonly string _script;

        protected ScriptMigration(string script) => _script = script;

        protected override void Up()
        {
            ExecuteEmbeddedScript(_script);
        }

        protected override void Down() => throw new NotImplementedException();

        protected void ExecuteEmbeddedScript(string path)
        {
            var effectivePath = $"{typeof(ScriptMigration).Assembly.GetName().Name}.Migrations.src.{path.Replace("\\", "/").Replace("/", ".")}";
            
            using (var stream = typeof(ScriptMigration).Assembly.GetManifestResourceStream(effectivePath))
            using (var reader = new StreamReader(stream))
            {
                var script = reader.ReadToEnd();
                Execute(script);
            }
        }
    }
}