using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;

namespace _build;

partial class Build : NukeBuild
{
    Target PublishExe => _ => _
        .DependsOn(Compile)
        .DependsOn(Test)
        .Executes(() =>
        {
            var publishDir = _userConfiguration.DefaultPublishDir;
            foreach (var project in _userConfiguration.ExecutablePublishProjects)
            {
                Project projectString = Solution.GetProject(project);
                string version = projectString.GetProperty("version");
                // set the version properly 
                DotNetTasks.DotNetPublish(s => s
                    .SetProject(project)
                    .SetConfiguration(Configuration)
                    .SetFramework(Framework)
                    .SetOutput(_userConfiguration.DefaultPublishDir)
                    .EnableSelfContained()
                    .EnablePublishSingleFile()
                    .EnablePublishReadyToRun()
                    .EnableNoRestore()
                    .EnableNoBuild()
                    .SetVersion(version));

                // preferably do a git tag here somehow.
            }
        });
}