using Microsoft.Build.Construction;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitVersion;
using System;
using System.Linq;
using Serilog;

namespace _build;

partial class Build : NukeBuild
{
    Target PublishExe => _ => _
        .DependsOn(Compile)
        .DependsOn(Test)
        .Produces()
        .Executes(() =>
        {
            foreach (string project in _userConfiguration.ExecutablePublishProjects)
            {
                Project projectString = Solution.GetProject(project);
                var projectRootElement = ProjectRootElement.Open(projectString);
                ProjectPropertyElement versionElement = projectRootElement.AllChildren.First(e => e.ElementName == "VersionPrefix") as ProjectPropertyElement;
                Version version = new Version(versionElement.Value);

                DotNetTasks.DotNetPublish(s => s
                    .SetProject(projectString)
                    .SetConfiguration(Configuration)
                    .SetFramework(Framework)
                    .SetOutput(_userConfiguration.DefaultPublishDir)
                    .EnableSelfContained()
                    .EnablePublishSingleFile()
                    .EnablePublishReadyToRun()
                    .EnableNoRestore()
                    .EnableNoBuild()
                    .SetVersion(version.ToString()));

                try
                {
                    var output = GitTasks.Git($"tag {project}/{version.Major}.{version.Minor}/{version}", logOutput: true);
                }
                catch (Exception)
                {
                    Log.Warning("Could not create git tag.");
                }
            }
        });
}