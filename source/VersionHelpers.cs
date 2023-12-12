using System;
using System.Linq;
using Microsoft.Build.Construction;
using Nuke.Common.IO;

namespace _build
{
    class VersionHelpers
    {
        public static string SetVersion(AbsolutePath projectFile, DateTime timestamp, bool isProd)
        {
            string prefix = string.Empty;
            string revisionPart = string.Empty;
            var doc = ProjectRootElement.Open(projectFile);
            var versionPrefixElement = doc.AllChildren.FirstOrDefault(child =>child.ElementName == "VersionPrefix");
            if (versionPrefixElement is ProjectPropertyElement element)
            {
                var currentVersion = element.Value;
                prefix = PrepareVersionPrefix(currentVersion);
                element.Value = prefix;
            }
        
            var versionSuffixElement = doc.AllChildren.FirstOrDefault(child =>child.ElementName == "VersionSuffix");
            if (versionSuffixElement is ProjectPropertyElement suffixElement)
            {
                revisionPart = PrepareVersionSuffix(timestamp, isProd);
                suffixElement.Value = revisionPart;
            }

            doc.Save();
            return CombinePrefixAndSuffix(prefix, revisionPart, isProd);
        }

        static string CombinePrefixAndSuffix(string prefix, string suffix, bool isProd)
        {
            return isProd ? prefix :  $"{prefix}.{suffix}";
        }

        static string PrepareVersionPrefix(string existingPrefix)
        {
            DateTime today = DateTime.Today;
            string prefix = today.ToString("yy.MM.");
            if (existingPrefix.StartsWith(prefix))
            {
                string buildNumberString = existingPrefix.Substring((existingPrefix.Length - 2));
                if (int.TryParse(buildNumberString, out int buildNumber))
                {
                    int newBuildNumber = buildNumber++;
                    return $"{prefix}{newBuildNumber:00}";
                }
            }

            return $"{prefix}01";
        }

        static string PrepareVersionSuffix(DateTime timestamp, bool isProd)
        {
            string revisionPart = timestamp.ToString("yyyyMMddTHHmm");
            return isProd
                ? string.Empty
                : revisionPart;
        }

        public static string GetVersionFromProject(AbsolutePath projectFile, bool isProd)
        {
            string prefix = string.Empty;
            string revisionPart = string.Empty;
            var doc = ProjectRootElement.Open(projectFile);
            var versionPrefixElement = doc.AllChildren.FirstOrDefault(child =>child.ElementName == "VersionPrefix");
            if (versionPrefixElement is ProjectPropertyElement element)
            {
                prefix = element.Value;
            }
        
            var versionSuffixElement = doc.AllChildren.FirstOrDefault(child =>child.ElementName == "VersionSuffix");
            if (versionSuffixElement is ProjectPropertyElement suffixElement)
            {
                revisionPart = suffixElement.Value;
            }

            return CombinePrefixAndSuffix(prefix, revisionPart, isProd);
        }
    }
}
