using Microsoft.Extensions.Logging;

namespace FilePacksLoader.Files;

public class FilesPacksSource<ContextT> : IDataPacksSource
    where ContextT : class, new()
{
    private readonly IDataSerializer _serializer;
    private readonly ILogger? _logger;

    private bool _isUseWatcher = false;
    private FileSystemWatcher? _watcher;

    public string Path { get; }

    public event EventHandler<IPackUpdatedEventArgs>? OnPackUpdated;

    public FilesPacksSource(string path, IDataSerializer serializer, ILogger? logger = null)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException($"'{path}' not found");
        Path = path;
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _logger = logger;

    }

    public IEnumerable<string> GetKeys()
    {
        return Directory.GetDirectories(Path, "*", SearchOption.TopDirectoryOnly).Select(p => System.IO.Path.GetFullPath(p));
    }

    public IDataLoader? GetLoader(string key)
    {
        if (!Directory.Exists(key))
            return null;
        var loader = new FilesDataLoader(key, _serializer, _logger);
        if (_isUseWatcher)
            loader.UseWatcher();
        return loader;
    }

    public FilesPacksSource<ContextT> UseWatcher()
    {
        if (_isUseWatcher)
            throw new InvalidOperationException();

        _isUseWatcher = true;
        _watcher = new FileSystemWatcher(Path);

        _watcher.Created += Watcher_Changed;
        _watcher.Deleted += Watcher_Changed;
        _watcher.Changed += Watcher_Changed;
        _watcher.Renamed += Watcher_Changed;

        _watcher.IncludeSubdirectories = false;
        _watcher.EnableRaisingEvents = true;

        return this;
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
