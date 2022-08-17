using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FilePacksLoader.Files.Json;

public abstract class JsonFilesPack<ContextT> : FilesPack<ContextT> where ContextT : class, new()
{
    private readonly JsonSerializerOptions? _options;

    public JsonFilesPack(string path, JsonSerializerOptions? options = null, ILogger? logger = null) : base(path, logger)
    {
        _options = options;
    }

    public override (string path, object? value) Parse(string propertyName, Type type)
    {
        var path = Path + "/" + propertyName + ".json";
        if (!File.Exists(path))
            return (path, null);
        string json = string.Empty;
        for (var i = 0; i < 3; i++)
            try
            {
                json = File.ReadAllText(path);
            }
            catch (Exception) when (i < 2)
            {
                Task.Delay(1000).Wait();
            }
        return (path, JsonSerializer.Deserialize(json, type, _options));
    }
}
