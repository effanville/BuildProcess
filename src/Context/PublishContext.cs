using System;

using Cake.Core.IO;

namespace _build.Context;

public sealed class PublishContext
{
    readonly DirectoryPath _rootDirectory;

    public DirectoryPath PublishDir { get; }
    public DirectoryPath BinOutput => PublishDir.Combine("bin");
    public DirectoryPath PackageOutput => PublishDir.Combine("package");

    public ProjectContext[] NugetPublishProjects { get; }

    public ProjectContext[] ExeProjects { get; }

    public PublishContext(DirectoryPath rootDirectory, DirectoryPath publishDir, string[] nugetPackages, string[] exeProjects)
    {
        _rootDirectory = rootDirectory;
        PublishDir = publishDir;
        if (nugetPackages != null && nugetPackages.Length > 0)
        {
            NugetPublishProjects = new ProjectContext[nugetPackages.Length];
            for (int i = 0; i < nugetPackages.Length; i++)
            {
                NugetPublishProjects[i] = new ProjectContext(rootDirectory, nugetPackages[i]);
            }
        }
        else
        {
            NugetPublishProjects = Array.Empty<ProjectContext>();
        }

        if (exeProjects != null && exeProjects.Length > 0)
        {
            ExeProjects = new ProjectContext[exeProjects.Length];
            for (int i = 0; i < exeProjects.Length; i++)
            {
                ExeProjects[i] = new ProjectContext(rootDirectory, exeProjects[i]);
            }
        }
        else
        {
            ExeProjects = Array.Empty<ProjectContext>();
        }
    }
}
