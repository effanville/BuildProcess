using System;
using Nuke.Common;
using Nuke.Common.Tools.Git;
using Serilog;

namespace _build;

partial class Build
{
    Target GitTag => _ => _
        .OnlyWhenDynamic(() => TagRepo)
        .DependsOn(ReleaseNotes)
        .Executes(() =>
        {
            try
            {
                GitTasks.Git("config --global user.name 'Effanville'", RootDirectory, logOutput: true);
                GitTasks.Git("config --global user.email 'effanville@users.noreply.github.com'", RootDirectory,
                    logOutput: true);
                GitTasks.Git("add Directory.Build.props", RootDirectory, logOutput: true);
                GitTasks.Git("add *.csproj", RootDirectory, logOutput: true);
                GitTasks.Git("add CHANGELOG.md", RootDirectory, logOutput: true);
                GitTasks.Git("status", RootDirectory, logOutput: true);
                GitTasks.Git("commit -m \"release: Update version and produce Automated report.\"", RootDirectory, logOutput: true);
                foreach (string tag in TagVersions)
                {
                    Log.Information($"Tagging with version {tag}");
                    GitTasks.Git($"tag {tag}", RootDirectory, logOutput: true);
                }

                if (!PushTag)
                {
                    return;
                }

                Log.Information($"Pushing everything.");
                GitTasks.Git("push --tags", RootDirectory, logOutput: true);
            }
            catch (Exception)
            {
                Log.Warning("Could not create git tag.");
            }
        });
}