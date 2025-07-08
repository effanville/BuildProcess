using _build.Context;

using Cake.Frosting;

namespace _build.Tasks;

[TaskName(nameof(DiagnosticPrint))]
public sealed class DiagnosticPrint : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.Info($"IsProd={context.IsProd}");
    }
}
