using _build.Context;

using Cake.Frosting;

namespace _build.Tasks;

[TaskName(nameof(Default))]
[IsDependentOn(typeof(Release))]
public sealed class Default : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Info("Completed Default build task.");
    }
}
