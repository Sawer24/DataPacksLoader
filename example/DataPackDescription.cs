namespace JsonFilesPacksExample;

public class DataPackDescription
{
    public required string Name { get; init; }
    public required Version Version { get; init; }
    public Version? GameVersion { get; init; }
}
