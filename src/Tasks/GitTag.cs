using System;
using System.Collections.Generic;

using _build.Context;

using Cake.Core.Diagnostics;
using Cake.Frosting;
using Cake.Git;

namespace _build.Tasks;

[TaskName(nameof(GitTag))]
[IsDependentOn(typeof(ReleaseNotes))]
public sealed class GitTag : FrostingTask<BuildContext>
{
    private const string User = "Effanville";
    private const string Email = "effanville@users.noreply.github.com";
    public override bool ShouldRun(BuildContext context) => context.TagRepo;
    public override void Run(BuildContext context)
    {
        context.Info("Starting GitTag task ...");
        try
        {
            GitAliases.GitAdd(context, context.RootDirectory, "Directory.Build.props");
            GitAliases.GitAdd(context, context.RootDirectory, "*.csproj");
            GitAliases.GitAdd(context, context.RootDirectory, "CHANGELOG.md");
            GitAliases.GitCommit(context, context.RootDirectory, $"release: Version {context.VersionInfo.GlobalVersion} and changelog update", User, Email);

            foreach (KeyValuePair<string, string> tag in context.VersionInfo.TagVersions)
            {
                string tagValue = tag.Key == "GLOBAL" ? $"v{tag.Value}" : $"{tag.Key}/{tag.Value}";
                context.Info($"Tagging with version {tagValue}");
                GitAliases.GitTag(context, context.RootDirectory, tagValue, $"Release version {tagValue}", User, Email);
            }

            if (!context.PushTag)
            {
                return;
            }

            context.Info($"Pushing everything.");
            GitAliases.GitPush(context, context.RootDirectory); // need to check the tags get pushed
        }
        catch (Exception)
        {
            context.Warn("Could not create git tag.");
        }
        context.Info("Completed GitTag task");
    }
}
