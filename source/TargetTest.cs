using Nuke.Common;
using Nuke.Common.Tools.DotNet;

namespace _build;

partial class Build : NukeBuild
{
    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetOutput(RootDirectory / $"bin/{Configuration}")
                .SetFramework(Framework)
                .SetConfiguration(Configuration));
        });
}