using _build.Context;

using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Core.IO;
using Cake.Frosting;

namespace _build.Tasks;

[TaskName(nameof(Test))]
[IsDependentOn(typeof(Compile))]
public sealed class Test : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Info("Testing solution ...");
        context.Info($"RootDir={context.RootDirectory}");
        context.Info($"Runtime={context.Runtime}");
        context.Info($"Framework={context.Framework}");
        context.Info($"Configuration={context.BuildConfiguration}");
        DotNetTestSettings settings = new DotNetTestSettings()
        {
            Configuration = context.BuildConfiguration,
            Framework = context.Framework,
            NoBuild = true,
            NoRestore = true,
            Verbosity = EnumConverter.ConvertDotNet(context.Log.Verbosity)
        };
        settings.Loggers.Clear();
        settings.Loggers.Add("trx;LogFileName=test-results.trx");
        settings.Collectors.Clear();
        settings.Collectors.Add("XPlat Code Coverage");

        FilePath file = context.SolutionInfo.SolutionFilePath;
        DotNetAliases.DotNetTest(context, file.FullPath, settings);
        context.Info("Completed Testing projects");
    }
}
