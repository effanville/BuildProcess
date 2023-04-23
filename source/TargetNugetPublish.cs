﻿using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Serilog;
using System;

namespace _build;

partial class Build : NukeBuild
{
    Target PublishNuget => _ => _
        .DependsOn(Compile)
        .DependsOn(Test)
        .Produces(PackageOutput / "*")
        .Executes(() =>
        {
            string publishDir = _userConfiguration.DefaultPublishDir;
            foreach (string project in _userConfiguration.NugetPackageProjects)
            {
                Project projectString = Solution.GetProject(project);
                var (csprojVersion, versionString) = VersionHelpers.GetVersionFromProject(projectString);

                DotNetTasks.DotNetPack(s => s
                    .SetProject(projectString)
                    .SetConfiguration(Configuration)
                    .SetRuntime(Runtime)
                    .SetOutputDirectory(PackageOutput)
                    .SetAssemblyVersion(versionString.ToString())
                    .EnableNoRestore()
                    .EnableNoBuild());

                try
                {
                    var output = GitTasks.Git($"tag {project}/{csprojVersion.Major}.{csprojVersion.Minor}/{versionString}",
                        logOutput: true,
                        workingDirectory: RootDirectory);
                }
                catch (Exception)
                {
                    Log.Warning("Could not create git tag.");
                }
            }
        });
}