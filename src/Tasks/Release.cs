using _build.Context;

using Cake.Frosting;

namespace _build.Tasks;

[TaskName(nameof(Release))]
[IsDependentOn(typeof(GitTag))]
public sealed class Release : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Info("Completed Release build task.");
    }
}
