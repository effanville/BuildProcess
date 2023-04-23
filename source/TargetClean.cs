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
            FileSystemTasks.EnsureCleanDirectory(BinDir);
            FileSystemTasks.EnsureCleanDirectory(RootDirectory / PublishDir);
            DotNetTasks.DotNetClean(c => c.SetProject(Solution));
            Log.Logger.Information("Completed cleaning projects.");
        });
}