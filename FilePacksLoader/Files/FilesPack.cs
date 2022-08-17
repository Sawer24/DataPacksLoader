using Microsoft.Extensions.Logging;

namespace FilePacksLoader.Files;

public abstract class FilesPack<ContextT> : DataPack<ContextT> where ContextT : class, new()
{
    private FileSystemWatcher? _watcher;
    private Dictionary<string, string>? _filePropertyPairs;

    public string Path { get; }

    public FilesPack(string path, ILogger? logger = null) : base(logger)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException($"'{path}' not found");
        Path = path;
    }

    public override object? GetData(string propertyKey, string propertyName, Type type)
    {
        var (path, value) = Parse(propertyName, type);
        if (_filePropertyPairs != null)
            _filePropertyPairs[System.IO.Path.GetFullPath(path)] = propertyKey;
        return value;
    }

    public abstract (string path, object? value) Parse(string propertyName, Type type);
    
    public void UseWatcher()
    {
        if (_watcher != null)
            return;
        _watcher = new FileSystemWatcher(Path);
        _filePropertyPairs = new Dictionary<string, string>();

        _watcher.Created += Watcher_Changed;
        _watcher.Deleted += Watcher_Changed;
        _watcher.Changed += Watcher_Changed;
        _watcher.Renamed += Watcher_Changed;

        _watcher.IncludeSubdirectories = true;
        _watcher.EnableRaisingEvents = true;
    }

    private void Watcher_Changed(object sender, FileSystemEventArgs e)
    {
        if (_filePropertyPairs?.TryGetValue(System.IO.Path.GetFullPath(e.FullPath), out var property) ?? false)
            Update(property);
    }
}
