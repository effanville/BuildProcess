using _build.Context;

using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Clean;
using Cake.Core.IO;
using Cake.Frosting;

namespace _build.Tasks;

[TaskName(nameof(Clean))]
[IsDependentOn(typeof(DiagnosticPrint))]
public sealed class Clean : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Info("Cleaning projects ...");
        context.Info($"RootDirectory={context.RootDirectory}");
        context.Info($"BuildConfiguration={context.BuildConfiguration.Value}");
        DirectoryPath binDir = context.BinDirectory.Combine(context.BuildConfiguration.Value);
        context.CleanDirectory(binDir);

        context.Info($"binDir={binDir}");
        string wd = context.SolutionInfo.SolutionWorkingDirectory.FullPath;
        context.Info($"WorkingDirectory={wd}");

        DotNetCleanSettings settings = new DotNetCleanSettings()
        {
            WorkingDirectory = wd,
            DiagnosticOutput = true,
            Verbosity = EnumConverter.ConvertDotNet(context.Log.Verbosity)
        };
        DotNetAliases.DotNetClean(context, context.SolutionInfo.SolutionFilePath.FullPath, settings);
        context.Info("Completed cleaning projects.");
    }
}

public static class EnumConverter
{
    public static DotNetVerbosity ConvertDotNet(Cake.Core.Diagnostics.Verbosity from)
    {
        return from switch
        {
            Cake.Core.Diagnostics.Verbosity.Quiet => DotNetVerbosity.Quiet,
            Cake.Core.Diagnostics.Verbosity.Minimal => DotNetVerbosity.Minimal,
            Cake.Core.Diagnostics.Verbosity.Normal => DotNetVerbosity.Normal,
            Cake.Core.Diagnostics.Verbosity.Verbose => DotNetVerbosity.Detailed,
            Cake.Core.Diagnostics.Verbosity.Diagnostic => DotNetVerbosity.Diagnostic,
            _ => DotNetVerbosity.Normal,
        };
    }
}