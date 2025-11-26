using System;
using System.Linq;

using _build.Config;

using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

namespace _build.Context;

public class BuildContext : FrostingContext
{
    public DirectoryPath RootDirectory { get; }
    public SolutionContext SolutionInfo { get; set; }
    public BuildVersionContext VersionInfo { get; set; } = new();
    public DirectoryPath BinDirectory => RootDirectory.Combine("bin");
    public FilePath NugetConfig => RootDirectory.CombineWithFilePath("nuget.config");

    public PublishContext PublishInfo { get; set; }
    public Configuration BuildConfiguration { get; }
    public string Framework { get; }
    public string Runtime { get; }
    public bool IsProd { get; }
    public bool TagRepo { get; }
    public bool PushTag { get; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        RootDirectory = context.MakeAbsolute(context.Directory("../../"));
        UserConfig? config = ConfigParser.ReadConfig(RootDirectory.CombineWithFilePath("build.config").FullPath);
        if (config == null)
        {
            throw new ArgumentException($"{nameof(UserConfig)} not specified.");
        }

        SolutionInfo = new SolutionContext(RootDirectory, config?.SolutionName);

        BuildConfiguration = context.Arguments.GetArgument("configuration") ?? config?.BuildConfiguration;
        Framework = context.Arguments.GetArgument("framework") ?? config.Framework;
        DirectoryPath publishDir = (DirectoryPath)(context.Arguments.GetArgument("publishDir") ?? config.PublishDir);
        string nugetPublishDir = context.Arguments.GetArgument("publishDir") ?? $"{config.PublishDir}/{config.SolutionName}";

        Runtime = context.Arguments.GetArgument("runtime") ?? config.Runtime;
        IsProd = context.Arguments.HasArgument("isProd");

        TagRepo = context.Arguments.HasArgument("TagRepo") ? true : config.TagRepo;

        string[] nugetPackages = config.NugetPackageProjects;
        string[] exeProjects = config.ExecutablePublishProjects;

        DirectoryPath publishLocation = publishDir.IsRelative
            ? context.MakeAbsolute(RootDirectory.Combine(publishDir))
            : publishDir;

        PathCollection ex = context.GetPaths(publishLocation.FullPath);
        Path? path = ex.FirstOrDefault();
        var data = path as DirectoryPath;
        PublishInfo = new PublishContext(RootDirectory, data, nugetPackages, exeProjects);
    }
}
