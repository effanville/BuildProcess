using System;

using _build.Context;

using Cake.Frosting;

namespace _build;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .InstallTool(new Uri("nuget:?package=NuGet.CommandLine&version=5.9.1"))
            .Run(args);
    }
}
