using System;
using System.Collections.Generic;
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
                GitTasks.Git($"commit -m \"release: Version {GlobalVersion} and changelog update.\"", RootDirectory, logOutput: true);
                foreach (KeyValuePair<string, string> tag in TagVersions)
                {
                    string tagValue = tag.Key == "GLOBAL" ? $"v{tag.Value}" : $"{tag.Key}/{tag.Value}";
                    Log.Information($"Tagging with version {tagValue }");
                    GitTasks.Git($"tag {tagValue }", RootDirectory, logOutput: true);
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