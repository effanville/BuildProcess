using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.ProjectModel;

namespace _build;

partial class Build
{
    Target UpdateVersion => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DateTime utcTimeStamp = DateTime.UtcNow;
            var buildPropsFile = Solution.Directory / "Directory.Build.props";
            string globalVersion = VersionHelpers.SetVersion(buildPropsFile, utcTimeStamp, IsProd);
            GlobalVersion = globalVersion;
            TagVersions.Add($"v{globalVersion}");
            var versionedProjects = ExecutablePublishProjects.Union(NugetPackageProjects);
            foreach (string project in versionedProjects)
            {
                Project projectString = Solution.GetProject(project);
                string version = VersionHelpers.SetVersion(projectString?.Path, utcTimeStamp, IsProd);
                if (!string.Equals(version, globalVersion))
                {
                    TagVersions.Add($"{project}/version");
                }
            }
        });
}