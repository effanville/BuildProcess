using _build.Context;

using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.Restore;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace _build.Tasks;

[TaskName(nameof(Compile))]
[IsDependentOn(typeof(Clean))]
[IsDependentOn(typeof(UpdateVersion))]
public sealed class Compile : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Info("Compiling solution ...");
        context.Info($"RootDir={context.RootDirectory}");
        var nugetConfigFile = context.NugetConfig;

        context.Info($"NugetConfig={context.NugetConfig}");
        context.Info($"Runtime={context.Runtime}");
        context.Info($"Framework={context.Framework}");
        context.Info($"Configuration={context.BuildConfiguration}");

        var nugetSettings = new DotNetRestoreSettings()
        {
            ConfigFile = nugetConfigFile,
            Runtime = context.Runtime,
            DiagnosticOutput = true,
            Verbosity = EnumConverter.ConvertDotNet(context.Log.Verbosity)
        };
        DotNetAliases.DotNetRestore(context, nugetSettings);
        context.Info("Restore Complete");

        var settings = new DotNetBuildSettings()
        {
            WorkingDirectory = context.SolutionInfo.SolutionWorkingDirectory.FullPath,
            Configuration = context.BuildConfiguration,
            Framework = context.Framework,
            NoRestore = true,
            DiagnosticOutput = true,
            Verbosity = EnumConverter.ConvertDotNet(context.Log.Verbosity)
        };
        DotNetAliases.DotNetBuild(context, context.SolutionInfo.SolutionFilePath.FullPath, settings);
        context.Info("Completed compiling solution");
    }
}
