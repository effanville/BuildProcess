using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

namespace _build;

[GitHubActions(
    "continuous",
    GitHubActionsImage.WindowsLatest,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(Publish) },
    Submodules = GitHubActionsSubmodules.Recursive,
    AutoGenerate = true)]
partial class Build : NukeBuild
{
    static AbsolutePath BinDir => RootDirectory / "bin";
    static AbsolutePath BinOutput => RootDirectory / _userConfiguration.DefaultPublishDir / "bin";
    static AbsolutePath PackageOutput => RootDirectory / _userConfiguration.DefaultPublishDir / "package";

    readonly static UserConfiguration _userConfiguration = new UserConfiguration((RootDirectory / "build.config"));

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    [Parameter("Framework to build with - Current is net6.0.")]
    readonly string Framework = _userConfiguration.DefaultFramework;

    [Parameter()]
    readonly string Runtime = _userConfiguration.DefaultRuntime;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = _userConfiguration.DefaultConfiguration;

    public static int Main() => Execute<Build>(x => x.Publish);

    Target Publish => _ => _
        .DependsOn(PublishExe)
        .DependsOn(PublishNuget)
        .Executes();
}
