using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitVersion;

namespace _build;

[GitHubActions(
    "continuous",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(Compile) })]
partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    static AbsolutePath _sourceDirectory => RootDirectory / "src";
    static AbsolutePath _outputDirectory => RootDirectory / "output";
    static AbsolutePath _binDir => RootDirectory / "bin";

    readonly static UserConfiguration _userConfiguration = new UserConfiguration((RootDirectory / "build.config"));

    [GitVersion(NoCache = true, UpdateBuildNumber = true, Framework = "net6.0")]
    GitVersion _gitVersion;

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    [Parameter("Framework to build with - Current is net6.0.")]
    readonly string Framework = _userConfiguration.DefaultFramework;

    [Parameter()]
    readonly string Runtime = _userConfiguration.DefaultRuntime;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = _userConfiguration.DefaultConfiguration;

    public static int Main() => Execute<Build>(x => x.PublishExe, x => x.PublishNuget);
}
