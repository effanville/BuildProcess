using _build.Context;

using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Core.IO;
using Cake.Frosting;

namespace _build.Tasks;

[TaskName(nameof(PublishExe))]
[IsDependentOn(typeof(Compile))]
[IsDependentOn(typeof(Test))]
public sealed class PublishExe : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Info("Publishing exe packages ...");
        context.Info($"RootDir={context.RootDirectory}");
        context.Info($"Runtime={context.Runtime}");
        context.Info($"Framework={context.Framework}");
        context.Info($"Configuration={context.BuildConfiguration}");
        PublishContext publishContext = context.PublishInfo;

        context.Info($"PublishDir={publishContext.PublishDir.FullPath}");
        context.Info($"BinOutput={publishContext.BinOutput.FullPath}");
        foreach (var project in publishContext.ExeProjects)
        {
            context.Info($"Project={project}");
            if (!context.VersionInfo.TagVersions.TryGetValue(project.ProjectName, out string? versionString))
            {
                versionString = context.VersionInfo.GlobalVersion;
            }
            DirectoryPath outputDir = publishContext.PublishDir.Combine(context.Directory(versionString));

            DotNetPublishSettings settings = new DotNetPublishSettings()
            {
                Framework = context.Framework,
                Configuration = context.BuildConfiguration,
                Runtime = context.Runtime,
                OutputDirectory = outputDir,
                SelfContained = true,
                Verbosity = EnumConverter.ConvertDotNet(context.Log.Verbosity)
            };
            DotNetAliases.DotNetPublish(context, project.FilePath.FullPath, settings);

            FilePath zipLocation = context.PublishInfo.BinOutput.CombineWithFilePath($"{project.ProjectName}.zip");
            System.IO.Directory.CreateDirectory(context.PublishInfo.BinOutput.FullPath);
            ZipAliases.Zip(context, outputDir, zipLocation);
        }
    }
}
