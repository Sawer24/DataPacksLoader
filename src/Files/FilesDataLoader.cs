using Microsoft.Extensions.Logging;

namespace DataPacksLoader.Files;

public class FilesDataLoader : IDataLoader
{
    private readonly IDataSerializer _serializer;
    private readonly ILogger? _logger;

    private readonly bool _isUseWatcher;
    private readonly FileSystemWatcher? _watcher;
    private readonly Dictionary<string, string>? _filePropertyPairs;

    public string Path { get; }

    public event EventHandler<IDataUpdatedEventArgs>? OnDataUpdated;

    public FilesDataLoader(string path, bool isUseWatcher, IDataSerializer serializer, ILogger? logger = null)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException($"'{path}' not found");
        Path = path;
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _logger = logger;
        if (_isUseWatcher = isUseWatcher)
        {
            _watcher = new FileSystemWatcher(Path);
            _filePropertyPairs = new Dictionary<string, string>();

            _watcher.Created += Watcher_Changed;
            _watcher.Deleted += Watcher_Changed;
            _watcher.Changed += Watcher_Changed;
            _watcher.Renamed += Watcher_Changed;

            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
        }

    }

    public T? LoadData<T>(string key, IPropertyPolicy policy)
    {
        var path = Path + "/" + ((FilesPropertyPolicy)policy).FilePath;
        T? value;
        if (File.Exists(path))
        {
            Stream? stream = null;
            try
            {
                for (var i = 0; i < 3; i++)
                    try
                    {
                        stream = File.OpenRead(path);
                        break;
                    }
                    catch (Exception) when (i < 2)
                    {
                        Task.Delay(1000).Wait();
                    }
                value = _serializer.Deserialize<T>(stream!);
            }
            finally
            {
                stream?.Close();
                stream?.Dispose();
            }
        }
        else
            value = default;

        if (_isUseWatcher)
            _filePropertyPairs![System.IO.Path.GetFullPath(path)] = key;
        return value;
    }

    private void Watcher_Changed(object sender, FileSystemEventArgs e)
    {
        _logger?.LogDebug("Pack watcher noticed a change");
        if (_filePropertyPairs?.TryGetValue(System.IO.Path.GetFullPath(e.FullPath), out var property) ?? false)
            OnDataUpdated?.Invoke(this, new DataUpdatedEventArgs { Key = property });
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
