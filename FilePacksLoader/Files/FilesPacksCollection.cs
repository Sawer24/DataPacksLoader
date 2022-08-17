using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FilePacksLoader.Files;

public abstract class FilesPacksCollection<PackT, ContextT> : DataPacksCollection<PackT, ContextT>
    where PackT : FilesPack<ContextT>
    where ContextT : class, new()
{
    private static Func<string, ILogger?, PackT> GenerateCreator()
    {
        var pathParam = Expression.Parameter(typeof(string), "path");
        var loggerParam = Expression.Parameter(typeof(ILogger), "logger");
        return Expression.Lambda<Func<string, ILogger?, PackT>>(
               Expression.New(typeof(PackT).GetConstructor(new[] { typeof(string), typeof(ILogger) }) ?? throw new TypeInitializationException(typeof(PackT).FullName, null),
               pathParam, loggerParam),
        pathParam, loggerParam).Compile();
    }
    private static Func<string, ILogger?, PackT> _creator;

    private bool _isUseWatcher = false;
    private FileSystemWatcher? _watcher;

    public string Path { get; }

    public FilesPacksCollection(string path, ILogger? logger = null) : base(logger)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException($"'{path}' not found");
        Path = path;
    }

    public override void Load()
    {
        if (_dataPacks != null)
            throw new InvalidOperationException();

        var packPaths = Directory.GetDirectories(Path, "*", SearchOption.TopDirectoryOnly);

        _dataPacks = new List<PackT>();
        foreach (var packPath in packPaths)
        {
            var pack = CreatePackInstance(packPath, _logger);
            if (_isUseWatcher)
                pack.UseWatcher();
            pack.Load();
            _dataPacks.Add(pack);
        }
    }

    protected abstract PackT CreatePackInstance(string path, ILogger? logger);

    public void UseWatcher()
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
    }

    private void Watcher_Changed(object sender, FileSystemEventArgs e)
    {

    }
}
