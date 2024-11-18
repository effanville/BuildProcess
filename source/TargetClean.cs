using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Serilog;

namespace _build;

partial class Build : NukeBuild
{
    Target DiagnosticPrint => _ => _
        .Executes(() =>
        {
            Log.Information($"IsProd build: {IsProd}");
        });
    Target Clean => _ => _
        .DependsOn(DiagnosticPrint)
        .Executes(() =>
        {
            Log.Logger.Information("Cleaning projects.");
            BinDir.CreateOrCleanDirectory();
            (RootDirectory / PublishDir).CreateOrCleanDirectory();
            DotNetTasks.DotNetClean(c => c.SetProject(Solution));
            Log.Logger.Information("Completed cleaning projects.");
        });
}