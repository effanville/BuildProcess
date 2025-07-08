using _build.Context;

using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Pack;
using Cake.Frosting;

namespace _build.Tasks;

[TaskName(nameof(PublishNuget))]
[IsDependentOn(typeof(Compile))]
[IsDependentOn(typeof(Test))]
public sealed class PublishNuget : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Info("Publishing nuget packages ...");
        context.Info($"RootDir={context.RootDirectory}");
        context.Info($"Runtime={context.Runtime}");
        context.Info($"Framework={context.Framework}");
        context.Info($"Configuration={context.BuildConfiguration}");
        PublishContext publishContext = context.PublishInfo;

        context.Info($"PublishDir={publishContext.PublishDir.FullPath}");
        context.Info($"PackageOutput={publishContext.PackageOutput.FullPath}");
        foreach (ProjectContext project in publishContext.NugetPublishProjects)
        {
            context.Info($"Project={project}");
            DotNetPackSettings settings = new DotNetPackSettings()
            {
                OutputDirectory = publishContext.PackageOutput,
                Configuration = context.BuildConfiguration,
                Runtime = context.Runtime,
                NoBuild = true,
                NoRestore = true,
                Verbosity = EnumConverter.ConvertDotNet(context.Log.Verbosity)
            };

            DotNetAliases.DotNetPack(
                context,
                project.FilePath.FullPath,
                settings);
        }
    }
}
