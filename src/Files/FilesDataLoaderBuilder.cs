using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DataPacksLoader.Files;

public class FilesDataLoaderBuilder
{
    protected string _path;
    protected IDataSerializer? _serializer;
    protected ILogger? _logger;
    protected bool _isUseWatcher;

    public FilesDataLoaderBuilder(string path)
    {
        _path = path;
    }

    public FilesDataLoader Build()
    {
        if (_serializer == null)
            throw new InvalidOperationException("Serializer not specified");
        return new FilesDataLoader(_path, _isUseWatcher, _serializer, _logger);
    }

    public FilesDataLoaderBuilder UseSerializer(IDataSerializer serializer)
    {
        _serializer = serializer;
        return this;
    }

    public FilesDataLoaderBuilder UseLogger(ILogger? logger)
    {
        _logger = logger;
        return this;
    }

    public FilesDataLoaderBuilder UseJsonSerializer(JsonSerializerOptions? options = null)
    {
        _serializer = new JsonDataSerializer(options);
        return this;
    }

    public FilesDataLoaderBuilder UseWatcher(bool isUseWatcher = true)
    {
        _isUseWatcher = isUseWatcher;
        return this;
    }
}
