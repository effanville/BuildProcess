using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using System.IO;
using Nuke.Common.IO;
using Serilog;

namespace _build;

partial class Build
{
    Target PublishExe => _ => _
        .DependsOn(Compile)
        .DependsOn(Test)
        .Produces(BinOutput / "*")
        .Executes(() =>
        {
            foreach (string project in ExecutablePublishProjects)
            {
                Project projectString = Solution.GetProject(project);
                if (projectString == null)
                {
                    Log.Error($"Could not find project info for project: {project}");
                    continue;
                }

                if (!TagVersions.TryGetValue(projectString.Name, out string versionString))
                {
                    versionString = GlobalVersion;
                }

                string publishDirectory = RootDirectory / Path.Combine(PublishDir, projectString.Name, versionString);

                DotNetTasks.DotNetPublish(s => s
                    .SetProject(projectString)
                    .SetConfiguration(Configuration)
                    .SetFramework(Framework)
                    .SetOutput(publishDirectory)
                    .EnableSelfContained());

                string zipLocation = BinOutput / $"{projectString.Name}.zip";
                CompressionTasks.CompressZip(publishDirectory, zipLocation);
            }
        });
}