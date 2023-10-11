var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");
var buildDir = $"./Builds/Forest";
var buildCore = "./ui/Forest/Forest/Forest.csproj";
var buildControl = "./ui/Forest/Forest.Controls/Forest.Controls.csproj";
var buildShell = "./ui/Forest/Forest.Shell/Forest.Shell.csproj";
var outputDur = "./Builds/Nuget";
var testDir = "./tests/Forest.Tests/Forest.Tests.csproj";
#tool nuget:?package=NuGet.CommandLine&version=5.9.1
//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild(buildCore, new DotNetBuildSettings
    {
        Configuration = configuration,
    });
    DotNetBuild(buildControl, new DotNetBuildSettings
    {
        Configuration = configuration,
    });
    DotNetBuild(buildShell, new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetTest(testDir, new DotNetTestSettings()
    {
        Configuration = configuration,
        NoBuild = true,
    });
});


Task("Versioning")
    .IsDependentOn("Test")
    .Does(() =>
{

});

Task("Pack")
    .IsDependentOn("Versioning")
    .Does(() =>
{
    NuGetPack(buildCore, new NuGetPackSettings
    {
        OutputDirectory = outputDur,
        BasePath = buildDir
    });
    NuGetPack(buildControl, new NuGetPackSettings
    {
        OutputDirectory = outputDur,
        BasePath = buildDir
    });
    NuGetPack(buildShell, new NuGetPackSettings
    {
        OutputDirectory = outputDur,
        BasePath = buildDir
    });
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);