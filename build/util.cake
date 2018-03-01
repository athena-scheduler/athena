public void TestProject(FilePath project)
{
    EnsureDirectoryExists("_tests");

    var resultsDirectory = MakeAbsolute(Directory("_tests")).FullPath;
    var settings = new DotNetCoreTestSettings
    {
        ArgumentCustomization = args => args.AppendSwitch("--results-directory", resultsDirectory),
        Configuration = configuration,
        NoBuild = true,
        Logger = $"trx;LogFileName={project.GetFilenameWithoutExtension()}_results.trx"
    };
    var coverageSettings = new OpenCoverSettings
    {
       OldStyle = true,
       MergeOutput = true 
    }.WithFilter("+[Athena]*")
     .WithFilter("+[Athena.*]*")
     .WithFilter("-[Athena.Tests]*")
     .WithFilter("-[Athena.*.Tests]*");
    
    if(IsRunningOnWindows())
    {
        OpenCover(tool => {
                tool.DotNetCoreTest(project.FullPath, settings);
            },
            Directory(resultsDirectory) + File("coverage.xml"),
            coverageSettings
        );
    }
    else
    {
        Verbose("OpenCover is not supported on non-windows platforms. No code coverage will be generated");
        DotNetCoreTest(project.FullPath, settings);
    }
}