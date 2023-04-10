using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

namespace _build;

partial class Build : NukeBuild
{
    Target PublishNuget => _ => _
        .DependsOn(Compile)
        .DependsOn(Test)
        .Executes(() =>
        {
            var publishDir = _userConfiguration.DefaultPublishDir;
            foreach (var project in _userConfiguration.NugetPackageProjects)
            {
                Project projectString = Solution.GetProject(project);
                // need to set the version of the nupkg properly here.

                DotNetTasks.DotNetPack(s => s
                    .SetProject(project)
                    .SetConfiguration(Configuration)
                    .SetOutputDirectory(_userConfiguration.DefaultPublishDir)
                    .EnablePublishSingleFile()
                    .EnablePublishReadyToRun()
                    .EnableNoRestore()
                    .EnableNoBuild());
            }
        });
}