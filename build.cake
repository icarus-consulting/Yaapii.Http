#tool nuget:?package=GitReleaseManager
#addin "Cake.Figlet"

var target = Argument("target", "Default");
var configuration   = "Release";

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var buildArtifacts					= Directory("./artifacts");
var deployment						= Directory("./artifacts/deployment");
var version							= "1.0.0";

///////////////////////////////////////////////////////////////////////////////
// MODULES
///////////////////////////////////////////////////////////////////////////////
var yaapiiHttp                      = Directory("./src/Yaapii.Http");

// Unit tests
var yaapiiHttpTests                 = Directory("./tests/Test.Yaapii.Http");

///////////////////////////////////////////////////////////////////////////////
// CONFIGURATION VARIABLES
///////////////////////////////////////////////////////////////////////////////
var isAppVeyor                      = AppVeyor.IsRunningOnAppVeyor;
var isWindows                       = IsRunningOnWindows();

// DotNetCoreBuild
var netcore                         = "netcoreapp2.0";
var net                             = "net46";
var netstandard                     = "netstandard2.0";

// For GitHub release
var owner                           = "icarus-consulting";
var repository                      = "Yaapii.Http";

// For AppVeyor NuGetFeed
var nuGetSource = "https://ci.appveyor.com/nuget/icarus/api/v2/package";

// API key tokens for deployment
var gitHubToken                     = "";
var appVeyorToken                   = "";


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
	Information(Figlet("Yaapii.Http"));

    // Yaapii.Http
    var module = yaapiiHttp;
    DotNetCoreBuild(
        module,
        new DotNetCoreBuildSettings()
        {
            Configuration = configuration,
            Framework = netstandard,
            MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersionPrefix(version),
            NoRestore = true
        }
    );
});

///////////////////////////////////////////////////////////////////////////////
// Test
///////////////////////////////////////////////////////////////////////////////
Task("Test")
.IsDependentOn("Build")
.Does(() => 
{
    Information(Figlet("Test"));

    // Yaapii.Http
    var test = yaapiiHttpTests;
    DotNetCoreTest(
        test,
        new DotNetCoreTestSettings()
        {
            Configuration = configuration,
            NoRestore = true,
            Framework = netcore
        }
    );
});    

///////////////////////////////////////////////////////////////////////////////
// Nuget
///////////////////////////////////////////////////////////////////////////////
Task("Nuget")
.IsDependentOn("Clean")
.IsDependentOn("Version")
.Does(() =>
{
	Information(Figlet("NuGet"));
	
	var settings = new DotNetCorePackSettings()
    {
        Configuration = configuration,
        OutputDirectory = buildArtifacts,
	  	VersionSuffix = ""
    };
	settings.ArgumentCustomization = args => args.Append("--include-symbols");
    settings.MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersionPrefix(version);
	
    var module = yaapiiHttp;
    DotNetCorePack(
		module.ToString(),
		settings
    );
});

///////////////////////////////////////////////////////////////////////////////
// Credentials
///////////////////////////////////////////////////////////////////////////////
Task("Credentials")
.WithCriteria(() => isAppVeyor)
.Does(() =>
{
	Information(Figlet("Credentials"));
	
    gitHubToken = EnvironmentVariable("GITHUB_TOKEN");
    appVeyorToken = EnvironmentVariable("APPVEYOR_TOKEN");
});

///////////////////////////////////////////////////////////////////////////////
// GitHubRelease
///////////////////////////////////////////////////////////////////////////////
Task("GitHubRelease")
.WithCriteria(() => isAppVeyor && BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
.IsDependentOn("Nuget")
.IsDependentOn("Version")
.IsDependentOn("Credentials")
.Does(() => 
{
	Information(Figlet("GitHub Release"));
	
	GitReleaseManagerCreate(
        gitHubToken,
        owner,
        repository, 
        new GitReleaseManagerCreateSettings {
            Milestone         = version,
            Name              = version,
            Prerelease        = false,
            TargetCommitish   = "master"
        }
    );
          
    var nugets = string.Join(",", GetFiles("./artifacts/*.nupkg").Select(f => f.FullPath) );
	Information($"Release files:{Environment.NewLine}  " + nugets.Replace(",", $"{Environment.NewLine}  "));
	GitReleaseManagerAddAssets(
	    gitHubToken,
		owner,
		repository,
		version,
		nugets
    );
    GitReleaseManagerPublish(gitHubToken, owner, repository, version);
});

///////////////////////////////////////////////////////////////////////////////
// NuGetFeed
///////////////////////////////////////////////////////////////////////////////
Task("NuGetFeed")
.WithCriteria(() => isAppVeyor && BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
.IsDependentOn("Nuget")
.Does(() => 
{
	Information(Figlet("NuGetFeed"));
	
    var nugets = GetFiles($"{buildArtifacts.Path}/*.nupkg");
    foreach(var package in nugets)
	{
        if(!package.GetFilename().FullPath.Contains("symbols.nupkg"))
        {
            NuGetPush(
                package,
                new NuGetPushSettings {
                    Source = nuGetSource,
                    ApiKey = appVeyorToken
                }
            );
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
.IsDependentOn("Test")
.IsDependentOn("Nuget")
.IsDependentOn("Credentials")
.IsDependentOn("GitHubRelease")
.IsDependentOn("NuGetFeed")
;

RunTarget(target);
