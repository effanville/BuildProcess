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
                .SetFramework(Framework)
                .EnableNoRestore()
                .AddLoggers("trx;LogFileName=test-results.trx")
                .SetDataCollector("XPlat Code Coverage"));
        });
}