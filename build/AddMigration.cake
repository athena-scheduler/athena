using System.IO;

Task("AddMigration")
    .Does(() => 
{
    var name = Argument<string>("name");
    var prefix = DateTime.Now.ToString("yyyyMMddHHmmss");

    EnsureDirectoryExists("src/Athena.Data/Migrations/src");

    var script = "src/Athena.Data/Migrations/src/" + prefix + "_" + name + ".sql";
    var cs = "src/Athena.Data/Migrations/" + prefix + "_" + name + ".cs";

    // Backing Script
    if(!System.IO.File.Exists(script))
    {
        System.IO.File.Create(script);        
    }

    System.IO.File.WriteAllText(cs, $@"using SimpleMigrations;
namespace Athena.Data.Migrations
{{
    [Migration({prefix}, ""{name}"")]
    public class {name} : ScriptMigration
    {{
        public {name}() : base(""{prefix + "_" + name + ".sql"}"")
        {{
        }}
    }}
}}
");
});