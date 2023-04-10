using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;

using Nuke.Common.Tools.DotNet;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitVersion;

namespace _build;

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

    static UserConfiguration _userConfiguration = UserConfiguration.ReadConfig((RootDirectory / "build.config").Name);

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
