using System.IO;
using System.Text.Json;

namespace _build.Config;

/// <summary>
/// Class for all configuration information 
/// </summary>
public sealed class UserConfig
{
    public string SolutionName { get; set; }
    public string[] ExecutablePublishProjects { get; set; }

    public string[] NugetPackageProjects { get; set; }

    public string BuildConfiguration { get; set; }

    public Configuration DefaultConfiguration => BuildConfiguration == "Release" ? Configuration.Release : Configuration.Debug;

    public string Framework { get; set; }

    public string Runtime { get; set; }

    public string PublishDir { get; set; }

    public bool TagRepo { get; set; } = true;
    public bool PushTag { get; set; } = false;
}

public static class ConfigParser
{
    public static UserConfig? ReadConfig(string configFilePath)
    {
        using (StreamReader reader = new StreamReader(configFilePath))
        {
            // Read the stream as a string.
            string text = reader.ReadToEnd();

            UserConfig? config = JsonSerializer.Deserialize<UserConfig>(text);
            return config;
        }
    }
}