using System;
using System.Linq;

using _build.Context;

using Cake.Frosting;

namespace _build.Tasks;

[TaskName(nameof(UpdateVersion))]
[IsDependentOn(typeof(Clean))]
public sealed class UpdateVersion : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Info("Updating version ...");
        DateTime utcTimeStamp = DateTime.UtcNow;
        context.Info($"CurrentTime={utcTimeStamp:yyyyMMddTHH:mm:ss.fff}");
        var buildPropsFile = context.SolutionInfo.SolutionWorkingDirectory.CombineWithFilePath("Directory.Build.props");
        context.Info($"BuildPropsFile={buildPropsFile}");

        string globalVersion = VersionHelpers.SetVersion(buildPropsFile.FullPath, utcTimeStamp, context.IsProd);
        context.VersionInfo.GlobalVersion = globalVersion;
        context.VersionInfo.TagVersions.Add("GLOBAL", globalVersion);
        context.Info($"Global version is '{globalVersion}'.");
        var versionedProjects = context.PublishInfo.ExeProjects.Union(context.PublishInfo.NugetPublishProjects);

        foreach (ProjectContext project in versionedProjects)
        {
            string version = VersionHelpers.SetVersion(project.FilePath.FullPath, utcTimeStamp, context.IsProd);
            if (string.IsNullOrEmpty(version) || string.Equals(version, globalVersion))
            {
                continue;
            }

            context.Info($"Specific project version is '{project.ProjectName}/{version}'.");
            context.VersionInfo.TagVersions.Add(project.ProjectName, version);
        }

        context.Info("Updated version ...");
    }
}