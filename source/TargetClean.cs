using Nuke.Common;
using Nuke.Common.IO;
using Serilog;

namespace _build;

partial class Build : NukeBuild
{
    Target Print => _ => _
        .Executes(() =>
        {
            Log.Information("GitVersion = {Value}", _gitVersion.MajorMinorPatch);
        });
    Target Clean => _ => _
    .DependsOn(Print)
        .Executes(() =>
        {
            Log.Logger.Information("Cleaning projects.");
            FileSystemTasks.EnsureCleanDirectory(_binDir);
            //DotNetTasks.DotNetClean(c => c.SetProject(Solution));
            Log.Logger.Information("Completed cleaning projects.");
        });
}