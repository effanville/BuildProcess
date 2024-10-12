using System.Collections.Generic;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

namespace _build;

[GitHubActions(
    "continuous",
    GitHubActionsImage.WindowsLatest,
    On = new[] { GitHubActionsTrigger.PullRequest, GitHubActionsTrigger.WorkflowDispatch },
    InvokedTargets = new[] { nameof(Publish) },
    Submodules = GitHubActionsSubmodules.Recursive,
    FetchDepth = 0,
    AutoGenerate = false)]
[GitHubActions(
    "ReleaseBuild",
    GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[] { "master" },
    InvokedTargets = new[] { nameof(Publish) },
    Submodules = GitHubActionsSubmodules.Recursive,
    FetchDepth = 0,
    
    AutoGenerate = false)]
partial class Build : NukeBuild
{
    static AbsolutePath BinDir => RootDirectory / "bin";
    AbsolutePath BinOutput => RootDirectory / PublishDir / "bin";
    AbsolutePath PackageOutput => RootDirectory / PublishDir / "package";

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    [Parameter("Framework to build with - Current is net7.0.")]
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

    [Parameter("Should this version be tagged")]
    readonly bool TagRepo;
    
    [Parameter("Should this tag be pushed to the remote")]
    readonly bool PushTag;
    
    string GlobalVersion;
    Dictionary<string, string> TagVersions = new();
    public static int Main() => Execute<Build>(x => x.Release);
}
