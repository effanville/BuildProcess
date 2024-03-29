﻿using System.IO;

using Newtonsoft.Json;

namespace _build;

/// <summary>
/// Class for all configuration information 
/// </summary>
public sealed class UserConfiguration
{
    public string[] ExecutablePublishProjects
    {
        get; set;
    }

    public string[] NugetPackageProjects
    {
        get; set;
    }

    public string DefaultBuildConfiguration
    {
        get; set;
    }

    public Configuration DefaultConfiguration => DefaultBuildConfiguration == "Release" ? Configuration.Release : Configuration.Debug;

    public string DefaultFramework
    {
        get; set;
    }

    public string DefaultRuntime
    {
        get; set;
    }

    public string DefaultPublishDir
    {
        get; set;
    }

    public UserConfiguration()
    {
    }

    public UserConfiguration(string configFilePath)
    {
        var config = ReadConfig(configFilePath);
        ExecutablePublishProjects = config.ExecutablePublishProjects;
        NugetPackageProjects = config.NugetPackageProjects;
        DefaultBuildConfiguration = config.DefaultBuildConfiguration;
        DefaultFramework = config.DefaultFramework;
        DefaultRuntime = config.DefaultRuntime;
        DefaultPublishDir = config.DefaultPublishDir;
    }

    public static UserConfiguration ReadConfig(string configFilePath)
    {
        using (StreamReader file = File.OpenText(configFilePath))
        {
            JsonSerializer serializer = new JsonSerializer();
            return (UserConfiguration)serializer.Deserialize(file, typeof(UserConfiguration));
        }
    }
}