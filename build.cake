#addin "nuget:?package=Cake.Docker&version=0.8.2"
#addin "nuget:?package=Cake.NPM&version=0.12.1"
#addin "nuget:?package=Cake.Codecov&version=0.3.0"

#tool "nuget:?package=Codecov&version=1.0.3"

#l "./build/AddMigration.cake"
#l "./build/util.cake"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

const string SOLUTION = "./Athena.sln";
const string MINICOVER = "./minicover/minicover.csproj";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var imageTag = Argument("imageTag", "latest");
var coverageThreshold = Argument("coverageThreshold", 60.0f);

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
    DotNetCoreRestore(MINICOVER);
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
        Configuration = configuration,
        NoRestore = true
    });
});

Task("Test::Prepare")
    .IsDependentOn("Clean::Test")
    .IsDependentOn("Build")
    .Does(() => 
{
    EnsureDirectoryExists("./_tests");

    DotNetCoreTool(MINICOVER, "minicover", "instrument --workdir ../ --assemblies test/**/bin/**/*.dll --exclude-assemblies test/**/bin/**/*Test*.dll --sources src/**/*.cs");
    DotNetCoreTool(MINICOVER, "minicover", "reset");
});

Task("Test::Unit")
    .IsDependentOn("Test::Prepare")
    .Does(() =>
{
    foreach(var project in GetFiles("./test/Unit/**/*.csproj"))
    {
        DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings
        {
            Configuration = configuration,
            NoRestore = true,
            NoBuild = true
        });
    }
});

Task("Test::Integration")
    .IsDependentOn("Test::Prepare")
    .Does(() => 
{
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

    try
    {
        foreach(var project in GetFiles("./test/Integration/**/*.csproj"))
        {
            DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings
            {
                Configuration = configuration,
                NoRestore = true,
                NoBuild = true
            });
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
    .IsDependentOn("Test::Integration")
    .Does(() =>
{
    DotNetCoreTool(MINICOVER, "minicover", "uninstrument --workdir ../");

    Information("Coverage Results: ");
    AllowFailure(() => DotNetCoreTool(MINICOVER, "minicover", $"report --workdir ../ --threshold {coverageThreshold.ToString("0.00")}"));
    AllowFailure(() => DotNetCoreTool(MINICOVER, "minicover", $"opencoverreport --workdir ../ --output ./_tests/coverage.xml --threshold {coverageThreshold.ToString("0.00")}"));
});

Task("Dist")
    .IsDependentOn("Clean::Dist")
    .IsDependentOn("Test")
    .Does(() => 
{
    EnsureDirectoryExists("./_dist");

    DotNetCorePublish("./src/Athena/Athena.csproj", new DotNetCorePublishSettings
    {
        Configuration = configuration,
        NoRestore = true,
        OutputDirectory = MakeAbsolute(Directory("./_dist/Athena")).FullPath
    });

    DotNetCorePublish("./src/Athena.Importer/Athena.Importer.csproj", new DotNetCorePublishSettings
    {
        Configuration = configuration,
        NoRestore = true,
        OutputDirectory = MakeAbsolute(Directory("./_dist/Athena.Importer")).FullPath
    });
});

Task("Docker::Build")
    .IsDependentOn("Dist")
    .Does(() =>
{
    DockerBuild(new DockerImageBuildSettings
    {
        Tag = new []{$"athenascheduler/athena:{imageTag}"}
    }, ".");

    DockerBuild(new DockerImageBuildSettings
    {
        Tag = new []{$"athenascheduler/importer:{imageTag}"},
        File = "./src/Athena.Importer/Dockerfile"
    }, ".");

    DockerBuild(new DockerImageBuildSettings
    {
        Tag = new []{$"athenascheduler/db:{imageTag}"},
        File = "./src/Athena.Importer/contrib/docker/Dockerfile"
    }, ".");
});

Task("Docker::Push")
    .IsDependentOn("Docker::Build")
    .Does(() =>
{
    DockerPush($"athenascheduler/athena:{imageTag}");
    DockerPush($"athenascheduler/importer:{imageTag}");
    DockerPush($"athenascheduler/db:{imageTag}");
});

Task("CodeCov::Publish")
    .IsDependentOn("Test")
    .Does(() =>
{
    Codecov("./_tests/coverage.xml");
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
