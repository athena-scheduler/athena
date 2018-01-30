#addin "nuget:?package=Cake.Docker&version=0.8.2"
#addin "nuget:?package=Cake.NPM&version=0.12.1"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

const string SOLUTION = "./Athena.sln";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean::Dist")
    .Does(() => 
{
    if(DirectoryExists("./_dist"))
    {
        DeleteDirectory("./_dist", new DeleteDirectorySettings
        {
            Recursive = true
        });
    }
});

Task("Clean::Test")
    .Does(() => 
{
    if(DirectoryExists("_tests"))
    {
        DeleteDirectory("_tests", new DeleteDirectorySettings
        {
            Recursive = true
        });
    }
});

Task("Clean")
    .IsDependentOn("Clean::Dist")
    .IsDependentOn("Clean::Test")
    .Does(() =>
{
    DotNetCoreClean(SOLUTION);
});

Task("Restore::NPM")
    .Does(() =>
{
    NpmInstall(new NpmInstallSettings
    {
        WorkingDirectory = "./src/Athena"
    });
});

Task("Restore::Nuget")
    .Does(() =>
{
    DotNetCoreRestore(SOLUTION);
});

Task("Restore")
    .IsDependentOn("Restore::NPM")
    .IsDependentOn("Restore::Nuget");

Task("Bundle")
    .IsDependentOn("Restore::NPM")
    .Does(() => 
{
    NpmRunScript(new NpmRunScriptSettings
    {
        WorkingDirectory = "./src/Athena",
        ScriptName = configuration == "Release" ? "bundle"  : "bundle::dev"
    });
});

Task("Build")
    .IsDependentOn("Restore")
    .IsDependentOn("Bundle")
    .Does(() =>
{
    DotNetCoreBuild(SOLUTION, new DotNetCoreBuildSettings {
        Configuration = configuration
    });
});

Task("Test::Unit")
    .IsDependentOn("Clean::Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    EnsureDirectoryExists("_tests");

    var settings = new DotNetCoreTestSettings
    {
        ArgumentCustomization = args => args.AppendSwitch("--results-directory", MakeAbsolute(Directory("_tests")).FullPath),
        Configuration = configuration,
        NoBuild = true,
        Logger = "trx;LogFileName=UnitTestResults.trx",
    };

    foreach(var project in GetFiles("./test/unit/**/*.csproj"))
    {
        DotNetCoreTest(project.FullPath, settings);
    }
});

Task("Test::Integration")
    .IsDependentOn("Clean::Test")
    .IsDependentOn("Build")
    .Does(() => 
{
    EnsureDirectoryExists("_tests");

    var container = Guid.Empty;
    if(HasArgument("UseDocker"))
    {
        DockerRun(new DockerContainerRunSettings
        {
            Name = (container = Guid.NewGuid()).ToString(),
            Detach = true,
            Publish = new [] {"5432:5432"},
            Rm = true
        }, "postgres:9.6-alpine", null, null);
        Information("Waiting 10 seconds to allow postgres to start...");
        System.Threading.Thread.Sleep(10000);
    }

    var settings = new DotNetCoreTestSettings
    {
        ArgumentCustomization = args => args.AppendSwitch("--results-directory", MakeAbsolute(Directory("_tests")).FullPath),
        Configuration = configuration,
        NoBuild = true,
        Logger = "trx;LogFileName=IntegrationTestResults.trx",
    };

    try
    {
        foreach(var project in GetFiles("./test/integration/**/*.csproj"))
        {
            DotNetCoreTest(project.FullPath, settings);
        }
    }
    finally
    {
        if(container != Guid.Empty)
        {
            DockerStop(container.ToString());
        }
    }
});

Task("Test")
    .IsDependentOn("Test::Unit")
    .IsDependentOn("Test::Integration");

Task("Dist")
    .IsDependentOn("Clean::Dist")
    .IsDependentOn("Test")
    .Does(() => 
{
});

Task("Docker")
    .IsDependentOn("Dist")
    .Does(() =>
{
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);