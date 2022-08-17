using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FilePacksLoader.Files.Json;

public class JsonFilesPacksCollection<ContextT> : FilesPacksCollection<JsonFilesPack<ContextT>, ContextT>
    where ContextT : class, new()
{
    private readonly JsonSerializerOptions? _options;

    public JsonFilesPacksCollection(string path, JsonSerializerOptions? options = null, ILogger? logger = null) : base(path, logger)
    {
        _options = options;
    }


}
