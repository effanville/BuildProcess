using Nuke.Common;
using Nuke.Common.Tools.Npm;

namespace _build;

partial class Build
{
    Target ReleaseNotes => _ => _
        .DependsOn(Publish)
        .Executes(() =>
        {
            var settings = new NpmInstallSettings();
            settings.AddPackages("git-cliff");
            NpmTasks.NpmInstall(settings);
            NpmTasks.Npm($"exec git-cliff -- --verbose --tag \"{GlobalVersion}\" --output CHANGELOG.md", RootDirectory);
        });
}