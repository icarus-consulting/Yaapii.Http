#addin "Cake.Figlet"

var target                  = Argument("target", "Default");
var configuration           = "Release";

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var buildArtifacts          = Directory("./artifacts");
var deployment              = Directory("./artifacts/deployment");
var version                 = "8.2.0";

///////////////////////////////////////////////////////////////////////////////
// MODULES
///////////////////////////////////////////////////////////////////////////////
var modules                 = Directory("./src");
var blacklistedModules      = new List<string>() { };

var tests                   = Directory("./tests");
var blacklistedUnitTests    = new List<string>() { };

///////////////////////////////////////////////////////////////////////////////
// CONFIGURATION VARIABLES
///////////////////////////////////////////////////////////////////////////////
var isAppVeyor              = AppVeyor.IsRunningOnAppVeyor;
var isWindows               = IsRunningOnWindows();

// For GitHub release
var owner                   = "icarus-consulting";
var repository              = "Yaapii.Http";

///////////////////////////////////////////////////////////////////////////////
// Clean
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() => 
    {
        Information(Figlet("Clean"));
    
        CleanDirectories(new DirectoryPath[] { buildArtifacts });
    });

///////////////////////////////////////////////////////////////////////////////
// Restore
///////////////////////////////////////////////////////////////////////////////
Task("Restore")
    .Does(() =>
    {
        Information(Figlet("Restore"));

        var projects = GetFiles("./**/*.csproj");
        foreach(var project in projects)
        {
            DotNetCoreRestore(project.GetDirectory().FullPath);
        }
    });

///////////////////////////////////////////////////////////////////////////////
// Version
///////////////////////////////////////////////////////////////////////////////
Task("Version")
    .WithCriteria(() => isAppVeyor && BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
    .Does(() => 
    {
        Information(Figlet("Version"));
    
        version = BuildSystem.AppVeyor.Environment.Repository.Tag.Name;
        Information($"Set version to '{version}'");
    });

///////////////////////////////////////////////////////////////////////////////
// Build
///////////////////////////////////////////////////////////////////////////////
Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Version")
    .Does(() =>
    {   
        Information(Figlet("Build"));

        var settings = 
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                NoRestore = true,
                MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersionPrefix(version)
            };
        var skipped = new List<string>();
        foreach(var module in GetSubDirectories(modules))
        {
            var name = module.GetDirectoryName();
            if(!blacklistedModules.Contains(name))
            {
                Information($"Building {name}");
            
                DotNetCoreBuild(
                    module.FullPath,
                    settings
                );
            }
            else
            {
                skipped.Add(name);
            }
        }
        if (skipped.Count > 0)
        {
            Warning("The following builds have been skipped:");
            foreach(var name in skipped)
            {
                Warning($"  {name}");
            }
        }
    });

///////////////////////////////////////////////////////////////////////////////
// Unit Tests
///////////////////////////////////////////////////////////////////////////////
Task("UnitTests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        Information(Figlet("Unit Tests"));

        var settings = 
            new DotNetCoreTestSettings()
            {
                Configuration = configuration,
                NoRestore = true
            };
        var skipped = new List<string>();
        foreach(var test in GetSubDirectories(tests))
        {
            var name = test.GetDirectoryName();
            if(blacklistedUnitTests.Contains(name))
            {
                skipped.Add(name);
            }
            else if(!name.StartsWith("TmxTest"))
            {
                Information($"Testing {name}");
                DotNetCoreTest(
                    test.FullPath,
                    settings
                );
            }
        }
        if (skipped.Count > 0)
        {
            Warning("The following tests have been skipped:");
            foreach(var name in skipped)
            {
                Warning($"  {name}");
            }
        }
    });

///////////////////////////////////////////////////////////////////////////////
// Default
///////////////////////////////////////////////////////////////////////////////
Task("Default")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Version")
.IsDependentOn("Build")
.IsDependentOn("UnitTests");

RunTarget(target);
