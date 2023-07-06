using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

namespace _build;

[GitHubActions(
    "continuous",
    GitHubActionsImage.WindowsLatest,
    On = new[] { GitHubActionsTrigger.PullRequest },
    InvokedTargets = new[] { nameof(Publish) },
    Submodules = GitHubActionsSubmodules.Recursive,
    FetchDepth = 0,
    AutoGenerate = true)]
[GitHubActions(
    "ReleaseBuild",
    GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[] { "master" },
    InvokedTargets = new[] { nameof(Publish) },
    Submodules = GitHubActionsSubmodules.Recursive,
    FetchDepth = 0,
    AutoGenerate = true)]
partial class Build : NukeBuild
{
    static AbsolutePath BinDir => RootDirectory / "bin";
    AbsolutePath BinOutput => RootDirectory / PublishDir / "bin";
    AbsolutePath PackageOutput => RootDirectory / PublishDir / "package";

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    [Parameter("Framework to build with - Current is net6.0.")]
    readonly string Framework;

    [Parameter()]
    readonly string Runtime;

    [Parameter("Configuration to build - Default is 'Debug'")]
    readonly Configuration Configuration;

    [Parameter("The projects to build as executables.")]
    readonly string[] ExecutablePublishProjects;

    [Parameter("The projects to build as nuget packages.")]
    readonly string[] NugetPackageProjects;

    readonly string PublishDir = "Publish";

    [Parameter("Is this a production release.")]
    readonly bool IsProd;

    public static int Main() => Execute<Build>(x => x.Publish);

    Target Publish => _ => _
        .DependsOn(PublishExe)
        .DependsOn(PublishNuget)
        .Executes();
}
