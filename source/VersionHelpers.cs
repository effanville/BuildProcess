using System;
using System.Linq;
using Microsoft.Build.Construction;
using Nuke.Common.ProjectModel;

namespace _build
{
    class VersionHelpers
    {
        public static (Version version, string versionString) GetVersionFromProject(Project project, bool isProd)
        {
            var projectRootElement = ProjectRootElement.Open(project);
            ProjectPropertyElement versionElement = projectRootElement.AllChildren.First(e => e.ElementName == "VersionPrefix") as ProjectPropertyElement;
            DateTime now = DateTime.UtcNow;
            string revisionPart = DateTime.UtcNow.ToString("yyyyMMddTHHmm");
            Version csprojVersion = new Version(versionElement.Value);
            string version = isProd
                ? versionElement.Value
                : $"{versionElement.Value}-{revisionPart}";
            return (csprojVersion, version);
        }
    }
}
