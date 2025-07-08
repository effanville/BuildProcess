namespace _build;

public class Configuration
{
    public string Value { get; private set; }
    public static Configuration Debug { get; } = new Configuration { Value = nameof(Debug) };
    public static Configuration Release { get; } = new Configuration { Value = nameof(Release) };

    public Configuration() { }
    public Configuration(string value) => Value = value;

    public static implicit operator string(Configuration configuration)
        => configuration.Value;

    public static implicit operator Configuration(string value)
        => new Configuration(value);
}
