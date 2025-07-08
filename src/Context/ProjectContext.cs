using System.IO;
using System.Linq;

using Cake.Core.IO;

namespace _build.Context;

public class ProjectContext
{
    public DirectoryPath FolderName { get; set; }
    public string ProjectName { get; set; }

    public FilePath FilePath { get; }

    public ProjectContext(DirectoryPath rootDirectory, string projectName)
    {
        ProjectName = projectName;
        FilePath = Directory.EnumerateFiles(rootDirectory.FullPath, $"{projectName}.csproj", SearchOption.AllDirectories).First();
        FolderName = FilePath.GetDirectory();
    }

    public override string ToString() => $"{ProjectName}, {FilePath}";
}
