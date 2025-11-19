using System.IO;
using System.Linq;

using Cake.Core.IO;
using System.IO;

namespace _build.Context;

public class SolutionContext
{
    private readonly string _solutionFileName;
    public string SolutionName { get; }
    public FilePath SolutionFilePath { get; }
    public DirectoryPath SolutionWorkingDirectory { get; }

    public SolutionContext(DirectoryPath rootDirectory, string solutionName)
    {
        _solutionFileName = solutionName;
        SolutionName = System.IO.Path.GetFileNameWithoutExtension(solutionName);
        SolutionFilePath = Directory.GetFiles(
            rootDirectory.FullPath,
            System.IO.Path.ChangeExtension(solutionName, ".sln"),
            SearchOption.AllDirectories)
            .First(x => x.EndsWith(".sln"));
        SolutionWorkingDirectory = System.IO.Path.GetDirectoryName(SolutionFilePath.FullPath);
    }
}
