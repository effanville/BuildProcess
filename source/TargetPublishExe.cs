using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using System.IO;
using Nuke.Common.IO;

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
                string versionString = VersionHelpers.GetVersionFromProject(projectString.Path, IsProd);

                string publishDirectory = RootDirectory / Path.Combine(PublishDir, projectString.Name, versionString);

                DotNetTasks.DotNetPublish(s => s
                    .SetProject(projectString)
                    .SetConfiguration(Configuration)
                    .SetFramework(Framework)
                    .SetOutput(publishDirectory)
                    .EnableSelfContained()
                    .EnablePublishSingleFile());

                string zipLocation = BinOutput / $"{projectString.Name}.zip";
                CompressionTasks.CompressZip(publishDirectory, zipLocation);
            }   
        });
}