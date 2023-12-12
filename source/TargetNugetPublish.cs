using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

namespace _build;

partial class Build
{
    Target PublishNuget => _ => _
        .DependsOn(Compile)
        //.DependsOn(Test)
        .Produces(PackageOutput / "*")
        .Executes(() =>
        {
            foreach (string project in NugetPackageProjects)
            {
                Project projectString = Solution.GetProject(project);
                DotNetTasks.DotNetPack(s => s
                    .SetProject(projectString)
                    .SetConfiguration(Configuration)
                    .SetRuntime(Runtime)
                    .SetOutputDirectory(PackageOutput)
                    .EnableNoRestore()
                    .EnableNoBuild());
            }
        });
}