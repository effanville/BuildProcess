using Nuke.Common;

namespace _build;

partial class Build
{
    Target Release => _ => _
        .DependsOn(GitTag)
        .Executes();
}