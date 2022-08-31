using Microsoft.Extensions.Logging;

namespace DataPacksLoader.Files;

public class FilesPacksSource : IDataPacksSource
{
    private readonly IDataSerializer _serializer;
    private readonly ILogger? _logger;

    private readonly bool _isUseWatcher;
    private readonly FileSystemWatcher? _watcher;
    private readonly Predicate<string>? _folgersFilter;

    public string Path { get; }

    public event EventHandler<IPackUpdatedEventArgs>? OnPackUpdated;

    public FilesPacksSource(string path, bool isUseWatcher, IDataSerializer serializer, Predicate<string>? folgersFilter = null, ILogger? logger = null)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException($"'{path}' not found");
        Path = path;
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _folgersFilter = folgersFilter;
        _logger = logger;
        if (_isUseWatcher = isUseWatcher)
        {
            _watcher = new FileSystemWatcher(Path);

            _watcher.Created += Watcher_Changed;
            _watcher.Deleted += Watcher_Changed;
            _watcher.Changed += Watcher_Changed;
            _watcher.Renamed += Watcher_Changed;

            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;
        }
    }

    public IEnumerable<string> GetKeys()
    {
        var folgers = Directory.GetDirectories(Path, "*", SearchOption.TopDirectoryOnly).Select(p => System.IO.Path.GetFullPath(p));
        if (_folgersFilter != null)
            return folgers.Where(f => _folgersFilter(f));
        return folgers;
    }

    public IDataLoader? GetLoader(string key)
    {
        if (!Directory.Exists(key))
            return null;
        var loader = new FilesDataLoader(key, _isUseWatcher, _serializer, _logger);
        return loader;
    }

    private void Watcher_Changed(object sender, FileSystemEventArgs e)
    {
        _logger?.LogDebug("The source watcher noticed a change");
        OnPackUpdated?.Invoke(this, new PackUpdatedEventArgs { PackKey = System.IO.Path.GetFullPath(e.FullPath), IsDelete = e.ChangeType == WatcherChangeTypes.Deleted });
    }

    private bool disposedValue;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _watcher?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
