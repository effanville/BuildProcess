using _build.Context;

using Cake.Frosting;

namespace _build.Tasks;

[TaskName(nameof(Publish))]
[IsDependentOn(typeof(PublishExe))]
[IsDependentOn(typeof(PublishNuget))]
public sealed class Publish : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Info("Completed Publishing task.");
    }
}
