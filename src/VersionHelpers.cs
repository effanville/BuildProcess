using System;
using System.Linq;

using Microsoft.Build.Construction;


namespace _build
{
    class VersionHelpers
    {
        public static string SetVersion(string projectFile, DateTime timestamp, bool isProd)
        {
            string prefix = string.Empty;
            string revisionPart = PrepareVersionSuffix(timestamp, isProd);
            string versionString = null;
            var doc = ProjectRootElement.Open(projectFile);
            var versionElementBase = doc.AllChildren.FirstOrDefault(child => child.ElementName == "Version");
            if (versionElementBase is ProjectPropertyElement versionElement)
            {
                var currentVersion = versionElement.Value;
                prefix = PrepareVersionPrefix(currentVersion, isProd);
                versionString = CombinePrefixAndSuffix(prefix, revisionPart, isProd);
                versionElement.Value = versionString;
            }

            var versionPrefixElement = doc.AllChildren.FirstOrDefault(child => child.ElementName == "VersionPrefix");
            if (versionPrefixElement is ProjectPropertyElement element)
            {
                var currentVersion = element.Value;
                prefix = PrepareVersionPrefix(currentVersion, isProd);
                element.Value = prefix;
            }

            var versionSuffixElement = doc.AllChildren.FirstOrDefault(child => child.ElementName == "VersionSuffix");
            if (versionSuffixElement is ProjectPropertyElement suffixElement)
            {
                suffixElement.Value = revisionPart;
            }

            doc.Save();
            return versionString;
        }

        static string CombinePrefixAndSuffix(string prefix, string suffix, bool isProd)
        {
            return isProd ? prefix : $"{prefix}-{suffix}";
        }

        static string PrepareVersionPrefix(string existingPrefix, bool isProd)
        {
            string currentPrefix = existingPrefix.Split("-")[0];
            DateTime today = DateTime.Today;
            string prefix = today.ToString("yy.MM.");
            if (existingPrefix.StartsWith(prefix))
            {
                string buildNumberString = currentPrefix.Substring((currentPrefix.Length - 2));
                if (int.TryParse(buildNumberString, out int buildNumber))
                {
                    return isProd
                        ? $"{prefix}{++buildNumber:00}"
                        : $"{prefix}{buildNumber:00}";
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
    }
}
