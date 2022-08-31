using Microsoft.Extensions.Logging;

namespace DataPacksLoader.Files;

public class FilesPacksSourceBuilder
{
    protected string _path;
    protected IDataSerializer? _serializer;
    protected Predicate<string>? _folgersFilter;
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
        return new FilesPacksSource(_path, _isUseWatcher, _serializer, _folgersFilter, _logger);
    }

    public FilesPacksSourceBuilder UseSerializer(IDataSerializer serializer)
    {
        _serializer = serializer;
        return this;
    }

    public FilesPacksSourceBuilder ClearFilters()
    {
        _folgersFilter = null;
        return this;
    }

    public FilesPacksSourceBuilder AddFilter(Predicate<string>? filter)
    {
        if (_folgersFilter == null)
            _folgersFilter = filter;
        else if (filter != null)
            _folgersFilter = (s) => _folgersFilter(s) && filter(s);
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
