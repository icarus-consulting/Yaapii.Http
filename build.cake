#tool nuget:?package=GitReleaseManager
#tool nuget:?package=OpenCover
#tool nuget:?package=xunit.runner.console
#tool nuget:?package=Codecov
#tool nuget:?package=ReportGenerator

var target = Argument("target", "Default");
var configuration   = Argument<string>("configuration", "Release");
var coverageReport = Argument<bool>("report", false);

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var buildArtifacts					= Directory("./artifacts/");

///////////////////////////////////////////////////////////////////////////////
// YAAPII MODULES
///////////////////////////////////////////////////////////////////////////////
var yaapiiHttp						= Directory("./src/Yaapii.Http");

var version						= "0.1.0";


///////////////////////////////////////////////////////////////////////////////
// CONFIGURATION VARIABLES
///////////////////////////////////////////////////////////////////////////////
var isAppVeyor          = AppVeyor.IsRunningOnAppVeyor;
var isWindows           = IsRunningOnWindows();
var netstandard         = "netstandard2.0";

// Important for github release
var owner = "icarus-consulting";
var repository = "Yaapii.Http";

var githubtoken = "";
var codecovToken = "";


///////////////////////////////////////////////////////////////////////////////
// Clean
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() =>
{
    CleanDirectories(new DirectoryPath[] { buildArtifacts });
});


// Restore nuget Packages
Task("Restore")
  .Does(() =>
{
	var projects = GetFiles("./**/*.csproj");

	foreach(var project in projects)
	{
	    DotNetCoreRestore(project.GetDirectory().FullPath);
  }
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
	Information("__   __                _ _ ");
	Information("\\ \\ / /_ _  __ _ _ __ (_|_)");
	Information(" \\ V / _` |/ _` | '_ \\| | |");
    Information("  | | (_| | (_| | |_) | | |");
    Information("  |_|\\__,_|\\__,_| .__/|_|_|");
	Information("                |_|");

    // Yaapii Modules
    DotNetCoreBuild(
        yaapiiHttp,
        new DotNetCoreBuildSettings()
        {
            Configuration = configuration,
            Framework = netstandard,
            MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersionPrefix(version)
        }
    );
});

///////////////////////////////////////////////////////////////////////////////
// Version
///////////////////////////////////////////////////////////////////////////////
Task("Version")
  .WithCriteria(() => isAppVeyor && BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
  .Does(() => 
{
    version = BuildSystem.AppVeyor.Environment.Repository.Tag.Name;
});

///////////////////////////////////////////////////////////////////////////////
// Pack
///////////////////////////////////////////////////////////////////////////////
Task("Pack")
    .IsDependentOn("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
	
	var settings = new DotNetCorePackSettings()
    {
        Configuration = configuration,
        OutputDirectory = buildArtifacts,
	  	VersionSuffix = ""
    };
   
	settings.MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersionPrefix(version);
	settings.ArgumentCustomization = args => args.Append("--include-symbols");

   if (isAppVeyor)
   {

       var tag = BuildSystem.AppVeyor.Environment.Repository.Tag;
       if(!tag.IsTag) 
       {
			settings.VersionSuffix = "build" + AppVeyor.Environment.Build.Number.ToString().PadLeft(5,'0');
       } 
	   else 
	   {     
			settings.MSBuildSettings = new DotNetCoreMSBuildSettings().SetVersionPrefix(tag.Name);
       }
   }

	
	DotNetCorePack(
		yaapiiHttp.ToString(),
		settings
    );
});


///////////////////////////////////////////////////////////////////////////////
// Release
///////////////////////////////////////////////////////////////////////////////
Task("GetAuth")
	.WithCriteria(() => isAppVeyor)
    .Does(() =>
{
    githubtoken = EnvironmentVariable("GITHUB_TOKEN");
	codecovToken = EnvironmentVariable("CODECOV_TOKEN");
});

Task("Release")
  .WithCriteria(() => isAppVeyor && BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
  .IsDependentOn("Version")
  .IsDependentOn("Pack")
  .IsDependentOn("GetAuth")
  .Does(() => {
     GitReleaseManagerCreate(githubtoken, owner, repository, new GitReleaseManagerCreateSettings {
            Milestone         = version,
            Name              = version,
            Prerelease        = false,
            TargetCommitish   = "master"
    });

	var nugetFiles = string.Join(",", GetFiles("./artifacts/**/*.nupkg").Select(f => f.FullPath) );
	Information("Nuget artifacts: " + nugetFiles);

	GitReleaseManagerAddAssets(
		githubtoken,
		owner,
		repository,
		version,
		nugetFiles
		);

	GitReleaseManagerPublish(githubtoken, owner, repository, version);
});


Task("Default")
  .IsDependentOn("Build")
  .IsDependentOn("Pack")
  .IsDependentOn("Release");

RunTarget(target);
