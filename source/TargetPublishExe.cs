using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitVersion;
using System;
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
                var (csprojVersion, versionString) = VersionHelpers.GetVersionFromProject(projectString);

                string publishDirectory = RootDirectory / Path.Combine(_userConfiguration.DefaultPublishDir, projectString.Name);

                DotNetTasks.DotNetPublish(s => s
                    .SetProject(projectString)
                    .SetConfiguration(Configuration)
                    .SetFramework(Framework)
                    .SetOutput(publishDirectory)
                    .EnableSelfContained()
                    .EnablePublishSingleFile()
                    .SetVersion(versionString.ToString()));

                string zipLocation = BinOutput / $"{projectString.Name}.zip";
                CompressionTasks.CompressZip(publishDirectory, zipLocation);
                try
                {
                    var output = GitTasks.Git($"tag {project}/{csprojVersion.Major}.{csprojVersion.Minor}/{versionString}",
                        logOutput: true,
                        workingDirectory: RootDirectory);
                }
                catch (Exception)
                {
                    Log.Warning("Could not create git tag.");
                }
            }
        });
}