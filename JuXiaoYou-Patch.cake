var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");
var buildDir = $"./Builds/JuXiaoYou";
var buildSolution = "./FutureGL.Studio.sln";
var outputDur = "./Builds/JuXiaoYou";
var cpyDir = @"E:\发行\橘小柚\3.0-Patch";
var testDir = "./tests/MigaDB.Tests/MigaDB.Tests.csproj";
var testDir2 = "./tests/MigaStudio.Tests/MigaStudio.Tests.csproj";
var issFileName = MakeAbsolute(Directory("JuXiaoYou-Patch.iss"));

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
    NuGetRestore(buildSolution);
    MSBuild(buildSolution, new MSBuildSettings
    {
        Verbosity = Verbosity.Minimal,
        Configuration = configuration,
        PlatformTarget = PlatformTarget.x86,
        MaxCpuCount = 6,
        NodeReuse = true
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    // DotNetTest(testDir, new DotNetTestSettings()
    // {
    //     Configuration = configuration,
    //     NoBuild = true,
    // });
    // DotNetTest(testDir2, new DotNetTestSettings()
    // {
    //     Configuration = configuration,
    //     NoBuild = true,
    // });
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
    //
    // 清除原先的内容
    Information($"清除目录: {cpyDir}");
    CleanDirectory(Directory(cpyDir));

    // 复制到指定的目录
    Information($"复制到目录: {cpyDir}");
    CopyDirectory(MakeAbsolute(Directory(buildDir)), Directory(cpyDir));

    //
    // 执行ISS脚本
    Information(@$"正在执行Inno Setup编译脚本, 参数为:/cc {issFileName}");
    StartProcess(@"C:\Program Files (x86)\Inno Setup 6\Compil32.exe", $"/cc {issFileName}");
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);