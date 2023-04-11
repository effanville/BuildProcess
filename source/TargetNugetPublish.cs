using Microsoft.Build.Construction;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Serilog;
using System;
using System.Linq;

namespace _build;

partial class Build : NukeBuild
{
    Target PublishNuget => _ => _
        .DependsOn(Compile)
        .DependsOn(Test)
        .Executes(() =>
        {
            string publishDir = _userConfiguration.DefaultPublishDir;
            foreach (string project in _userConfiguration.NugetPackageProjects)
            {
                Project projectString = Solution.GetProject(project);

                var projectRootElement = ProjectRootElement.Open(projectString);
                ProjectPropertyElement versionElement = projectRootElement.AllChildren.First(e => e.ElementName == "VersionPrefix") as ProjectPropertyElement;
                Version version = new Version(versionElement.Value);

                DotNetTasks.DotNetPack(s => s
                    .SetProject(projectString)
                    .SetConfiguration(Configuration)
                    .SetOutputDirectory(_userConfiguration.DefaultPublishDir)
                    .EnablePublishSingleFile()
                    .EnablePublishReadyToRun()
                    .SetAssemblyVersion(version.ToString())
                    .EnableNoRestore()
                    .EnableNoBuild());

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