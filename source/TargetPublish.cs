using Nuke.Common;

namespace _build;

partial class Build
{
    Target Publish => _ => _
        .DependsOn(PublishExe)
        .DependsOn(PublishNuget)
        .Executes();
}