namespace DataPacksLoader.Files;

public static class FilesPropertiesPolicyFactory
{
    public static FilesPropertiesPolicy Create(string extension) => new(extension);
    public static FilesPropertiesPolicy CreateText() => Create(".txt");
    public static FilesPropertiesPolicy CreateJson() => Create(".json");
}
