using _build.Context;

using Cake.Frosting;
using Cake.Npm;
using Cake.Npm.Exec;
using Cake.Npm.Install;

namespace _build.Tasks;

[TaskName(nameof(ReleaseNotes))]
[IsDependentOn(typeof(Publish))]
public sealed class ReleaseNotes : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var npmInstallSettings = new NpmInstallSettings()
            .AddPackage("git-cliff");
        NpmInstallAliases.NpmInstall(context, npmInstallSettings);

        var npmExecSettings = new NpmExecSettings()
        {
            WorkingDirectory = context.RootDirectory.FullPath,
            LogLevel = NpmLogLevel.Verbose,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            StandardErrorAction = msg => context.Info(msg),
            StandardOutputAction = msg => context.Info(msg),
        };
        npmExecSettings.ExecCommand = "git-cliff";
        npmExecSettings.Arguments.Add("--verbose");
        npmExecSettings.Arguments.Add("--tag");
        npmExecSettings.Arguments.Add(context.VersionInfo.GlobalVersion);
        npmExecSettings.Arguments.Add("--output");
        npmExecSettings.Arguments.Add("changelog.md");
        NpmExecAliases.NpmExec(context, npmExecSettings);
    }
}
