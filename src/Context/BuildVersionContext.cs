using System.Collections.Generic;

namespace _build.Context;

public class BuildVersionContext
{
    public string GlobalVersion { get; set; }
    public Dictionary<string, string> TagVersions { get; set; } = new();
}
