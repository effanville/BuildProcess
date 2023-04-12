using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
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
            FileSystemTasks.EnsureCleanDirectory(BinDir);
            FileSystemTasks.EnsureCleanDirectory(RootDirectory / _userConfiguration.DefaultPublishDir);
            DotNetTasks.DotNetClean(c => c.SetProject(Solution));
            Log.Logger.Information("Completed cleaning projects.");
        });
}