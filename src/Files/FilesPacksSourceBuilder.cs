using Microsoft.Extensions.Logging;

namespace DataPacksLoader.Files;

public class FilesPacksSourceBuilder
{
    protected string _path;
    protected IDataSerializer? _serializer;
    protected ILogger? _logger;
    protected bool _isUseWatcher;

    public FilesPacksSourceBuilder(string path)
    {
        _path = path;
    }

    public FilesPacksSource Build()
    {
        if (_serializer == null)
            throw new InvalidOperationException("Serializer not specified");
        return new FilesPacksSource(_path, _isUseWatcher, _serializer, _logger);
    }

    public FilesPacksSourceBuilder UseSerializer(IDataSerializer serializer)
    {
        _serializer = serializer;
        return this;
    }

    public FilesPacksSourceBuilder UseLogger(ILogger? logger)
    {
        _logger = logger;
        return this;
    }

    public FilesPacksSourceBuilder UseWatcher(bool isUseWatcher = true)
    {
        _isUseWatcher = isUseWatcher;
        return this;
    }
}
