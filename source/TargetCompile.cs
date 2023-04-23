using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

namespace _build;

partial class Build : NukeBuild
{
    Target Compile => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            string configFilePath = RootDirectory / "nuget.config";
            DotNetTasks.DotNetRestore(s => s
                .SetProjectFile(Solution)
                .SetRuntime(Runtime)
                .SetConfigFile(configFilePath));
            foreach (Project project in Solution.Projects)
            {
                DotNetTasks.DotNetBuild(s => s
                    .SetProjectFile(project)
                    .SetConfiguration(Configuration)
                    .SetFramework(Framework)
                    .EnableNoRestore());
            }
        });
}