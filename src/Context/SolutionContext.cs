using System.IO;
using System.Linq;

using Cake.Core.IO;

namespace _build.Context;

public class SolutionContext
{
    private readonly string _solutionFileName;
    public string SolutionName { get; }
    public FilePath SolutionFilePath { get; }
    public DirectoryPath SolutionWorkingDirectory { get; }

    public SolutionContext(DirectoryPath rootDirectory, string solutionFileName)
    {
        _solutionFileName = solutionFileName;
        SolutionName = System.IO.Path.GetFileNameWithoutExtension(solutionFileName);
        SolutionFilePath = Directory.EnumerateFiles(rootDirectory.FullPath, solutionFileName, SearchOption.AllDirectories).First();
        SolutionWorkingDirectory = System.IO.Path.GetDirectoryName(SolutionFilePath.FullPath);
    }
}
