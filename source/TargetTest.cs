using Nuke.Common;
using Nuke.Common.Tools.DotNet;

namespace _build;

partial class Build : NukeBuild
{
    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            foreach (var testProject in TestProjects)
            {
                var projectInfo = Solution.GetProject(testProject);
                DotNetTasks.DotNetTest(s => s
                    .SetProjectFile(projectInfo)
                    .SetFramework(Framework)
                    .EnableNoRestore()
                    .EnableNoBuild()
                );
            }
        });
}