#tool nuget:?package=GitReleaseManager
//#tool nuget:?package=OpenCover
//#tool nuget:?package=xunit.runner.console
//#tool nuget:?package=Codecov
//#tool nuget:?package=ReportGenerator

#tool nuget:?package=GitReleaseManager
#addin "Cake.Figlet"

var target = Argument("target", "Default");
var configuration   = "Release";

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var buildArtifacts					= Directory("./artifacts");
var deployment						= Directory("./artifacts/deployment");
var version							= "0.19.0";

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
var getTokensFromUser               = false; // Need for deployment from local machine after executing local obfuscation
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
    //var output = Directory($"{buildArtifacts.Path}/{module.Path.GetDirectoryName()}");
    DotNetCoreBuild(
        module,
        new DotNetCoreBuildSettings()
        {
            Configuration = configuration,
            Framework = netstandard,
            MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersionPrefix(version),
            //OutputDirectory = output,
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
	
    if(getTokensFromUser && !isAppVeyor)
    {
	    Console.Write("Enter GitHub API token: ");
	    gitHubToken = Console.ReadLine();
        Console.Write("Enter AppVeyor API token: ");
	    appVeyorToken = Console.ReadLine();
    }
    else
    {
        gitHubToken = EnvironmentVariable("GITHUB_TOKEN");
        appVeyorToken = EnvironmentVariable("APPVEYOR_TOKEN");
    }
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
        NuGetPush(
            package,
            new NuGetPushSettings {
                Source = nuGetSource,
                ApiKey = appVeyorToken
            }
        );
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



//var target = Argument("target", "Default");
//var configuration   = Argument<string>("configuration", "Release");
//var coverageReport = Argument<bool>("report", false);
//
/////////////////////////////////////////////////////////////////////////////////
//// GLOBAL VARIABLES
/////////////////////////////////////////////////////////////////////////////////
//var buildArtifacts					= Directory("./artifacts/");
//
/////////////////////////////////////////////////////////////////////////////////
//// YAAPII MODULES
/////////////////////////////////////////////////////////////////////////////////
//var yaapiiHttp						= Directory("./src/Yaapii.Http");
//
//var version						= "0.1.7";
//
//
/////////////////////////////////////////////////////////////////////////////////
//// CONFIGURATION VARIABLES
/////////////////////////////////////////////////////////////////////////////////
//var isAppVeyor          = AppVeyor.IsRunningOnAppVeyor;
//var isWindows           = IsRunningOnWindows();
//var netstandard         = "netstandard2.0";
//
//// Important for github release
//var owner = "icarus-consulting";
//var repository = "Yaapii.Http";
//
//var githubtoken = "";
//var codecovToken = "";
//
//
/////////////////////////////////////////////////////////////////////////////////
//// Clean
/////////////////////////////////////////////////////////////////////////////////
//Task("Clean")
//    .Does(() =>
//{
//    CleanDirectories(new DirectoryPath[] { buildArtifacts });
//});
//
//
//// Restore nuget Packages
//Task("Restore")
//  .Does(() =>
//{
//	var projects = GetFiles("./**/*.csproj");
//
//	foreach(var project in projects)
//	{
//	    DotNetCoreRestore(project.GetDirectory().FullPath);
//  }
//});
//
/////////////////////////////////////////////////////////////////////////////////
//// Build
/////////////////////////////////////////////////////////////////////////////////
//Task("Build")
//  .IsDependentOn("Clean")
//  .IsDependentOn("Restore")
//  .IsDependentOn("Version")
//  .Does(() =>
//{
//	Information("__   __                _ _ ");
//	Information("\\ \\ / /_ _  __ _ _ __ (_|_)");
//	Information(" \\ V / _` |/ _` | '_ \\| | |");
//    Information("  | | (_| | (_| | |_) | | |");
//    Information("  |_|\\__,_|\\__,_| .__/|_|_|");
//	Information("                |_|");
//
//    // Yaapii Modules
//    DotNetCoreBuild(
//        yaapiiHttp,
//        new DotNetCoreBuildSettings()
//        {
//            Configuration = configuration,
//            Framework = netstandard,
//            MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersionPrefix(version)
//        }
//    );
//});
//
/////////////////////////////////////////////////////////////////////////////////
//// Version
/////////////////////////////////////////////////////////////////////////////////
//Task("Version")
//  .WithCriteria(() => isAppVeyor && BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
//  .Does(() => 
//{
//    version = BuildSystem.AppVeyor.Environment.Repository.Tag.Name;
//});
//
/////////////////////////////////////////////////////////////////////////////////
//// Pack
/////////////////////////////////////////////////////////////////////////////////
//Task("Pack")
//    .IsDependentOn("Restore")
//    .IsDependentOn("Clean")
//    .Does(() =>
//{
//	
//	var settings = new DotNetCorePackSettings()
//    {
//        Configuration = configuration,
//        OutputDirectory = buildArtifacts,
//	  	VersionSuffix = ""
//    };
//   
//	settings.MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersionPrefix(version);
//	settings.ArgumentCustomization = args => args.Append("--include-symbols");
//
//   if (isAppVeyor)
//   {
//
//       var tag = BuildSystem.AppVeyor.Environment.Repository.Tag;
//       if(!tag.IsTag) 
//       {
//			settings.VersionSuffix = "build" + AppVeyor.Environment.Build.Number.ToString().PadLeft(5,'0');
//       } 
//	   else 
//	   {     
//			settings.MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersionPrefix(tag.Name);
//       }
//   }
//
//	
//	DotNetCorePack(
//		yaapiiHttp.ToString(),
//		settings
//    );
//});
//
//
/////////////////////////////////////////////////////////////////////////////////
//// Release
/////////////////////////////////////////////////////////////////////////////////
//Task("GetAuth")
//	.WithCriteria(() => isAppVeyor)
//    .Does(() =>
//{
//    githubtoken = EnvironmentVariable("GITHUB_TOKEN");
//	codecovToken = EnvironmentVariable("CODECOV_TOKEN");
//});
//
//Task("Release")
//  .WithCriteria(() => isAppVeyor && BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
//  .IsDependentOn("Version")
//  .IsDependentOn("Pack")
//  .IsDependentOn("GetAuth")
//  .Does(() => {
//     GitReleaseManagerCreate(githubtoken, owner, repository, new GitReleaseManagerCreateSettings {
//            Milestone         = version,
//            Name              = version,
//            Prerelease        = false,
//            TargetCommitish   = "master"
//    });
//
//	var nugetFiles = string.Join(",", GetFiles("./artifacts/**/*.nupkg").Select(f => f.FullPath) );
//	Information("Nuget artifacts: " + nugetFiles);
//
//	GitReleaseManagerAddAssets(
//		githubtoken,
//		owner,
//		repository,
//		version,
//		nugetFiles
//		);
//
//	GitReleaseManagerPublish(githubtoken, owner, repository, version);
//});
//
//
//Task("Default")
//  .IsDependentOn("Build")
//  .IsDependentOn("Pack")
//  .IsDependentOn("Release");
//
//RunTarget(target);
