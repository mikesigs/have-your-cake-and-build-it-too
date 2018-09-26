var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
Task("Build")
    .Does(() =>
{
    DotNetCoreBuild("HaveYourCakeAndBuildItToo.sln", new DotNetCoreBuildSettings {
        Configuration = configuration
    });
});
Task("UnitTests")
    .IsDependentOn("Build")
    .DoesForEach(GetFiles("./src/**/*.Test.csproj"), (project) =>
{
    DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings {
        Configuration = configuration,
        NoBuild = true
    });
});
Task("OctoPack")
    .IsDependentOn("UnitTests")
    .Does(() => {
        DotNetCorePublish("./src/HaveYourApi/HaveYourApi.csproj", new DotNetCorePublishSettings {
            Configuration = configuration,
            NoBuild = true,
            OutputDirectory = "./build/publish"
        });
        OctoPack("HaveYourApi", new OctopusPackSettings {
            BasePath = "./build/publish",
            OutFolder = "./build/pack"
        });
    });
Task("Default")
    .IsDependentOn("UnitTests")
    .Does(() => {
        Information("Hello Cake!");
    });
RunTarget(target);
