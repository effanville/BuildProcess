using Microsoft.Build.Construction;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitVersion;
using System;
using System.Linq;
using Serilog;
using System.IO;
using Nuke.Common.IO;

namespace _build;

partial class Build : NukeBuild
{
    Target PublishExe => _ => _
        .DependsOn(Compile)
        .DependsOn(Test)
        .Produces(BinOutput / "*")
        .Executes(() =>
        {
            foreach (string project in _userConfiguration.ExecutablePublishProjects)
            {
                Project projectString = Solution.GetProject(project);
                var projectRootElement = ProjectRootElement.Open(projectString);
                ProjectPropertyElement versionElement = projectRootElement.AllChildren.First(e => e.ElementName == "VersionPrefix") as ProjectPropertyElement;
                Version version = new Version(versionElement.Value);

                string publishDirectory = RootDirectory / Path.Combine(_userConfiguration.DefaultPublishDir, projectString);

                DotNetTasks.DotNetPublish(s => s
                    .SetProject(projectString)
                    .SetConfiguration(Configuration)
                    .SetFramework(Framework)
                    .SetOutput(publishDirectory)
                    .EnableSelfContained()
                    .EnablePublishSingleFile()
                    .EnablePublishReadyToRun()
                    .EnableNoRestore()
                    .EnableNoBuild()
                    .SetVersion(version.ToString()));

                string zipLocation = BinOutput / $"{projectString}.zip";
                CompressionTasks.CompressZip(publishDirectory, zipLocation);
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