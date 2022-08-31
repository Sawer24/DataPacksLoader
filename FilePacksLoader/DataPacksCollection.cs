using Microsoft.Extensions.Logging;
using System.Reflection;

namespace FilePacksLoader;

public class DataPacksCollection<ContextT> : IDataPacksCollection<ContextT>
    where ContextT : class, new()
{
    protected readonly IDataPacksSource _source;
    protected readonly IDataPackMapper<ContextT> _mapper;
    protected readonly ILogger? _logger;

    protected Dictionary<string, IDataPack<ContextT>>? _dataPacks;
    protected ContextT? _combinedContext;
    protected CombinedDataPack<ContextT>? _combinedPack;

    public IReadOnlyDictionary<string, IDataPack<ContextT>> DataPacks => _dataPacks?.AsReadOnly() ?? throw new InvalidOperationException();
    public ICombinedDataPack<ContextT> CombinedPack => _combinedPack ?? throw new InvalidOperationException();

    public event EventHandler<IDataUpdatedEventArgs>? OnDataUpdated;

    public DataPacksCollection(IDataPacksSource source, IDataPackMapper<ContextT> mapper, ILogger? logger = null)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
    }

    public IDataPacksCollection<ContextT> Load()
    {
        if (_dataPacks != null)
            throw new InvalidOperationException();

        _dataPacks = new Dictionary<string, IDataPack<ContextT>>();
        var keys = _source.GetKeys();
        foreach (var key in keys)
        {
            var loader = _source.GetLoader(key);
            if (loader == null)
                continue;

            var pack = new DataPack<ContextT>(loader, _mapper, _logger);
            pack.OnDataUpdated += Update;
            pack.Load();
            _dataPacks.Add(key, pack);
        }
        _source.OnPackUpdated += UpdatePack;
        _logger?.LogInformation("Pack collection has successfully loaded {count} packs", _dataPacks.Count);
        return this;
    }

    public IDataPacksCollection<ContextT> Combine()
    {
        if (_dataPacks == null)
            throw new InvalidOperationException();
        if (_combinedPack != null)
            throw new InvalidOperationException();

        var context = new ContextT();

        try
        {
            _mapper.CombineProperties()(_dataPacks.Select(p => p.Value.Data), context);
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException!;
        }
        _combinedContext = context;
        _combinedPack = new CombinedDataPack<ContextT>(context);
        _logger?.LogInformation("Pack collection successfully combined");
        return this;
    }

    private void UpdatePack(object? sender, IPackUpdatedEventArgs e)
    {
        if (_dataPacks == null)
            throw new InvalidOperationException();

        var pack = _dataPacks.GetValueOrDefault(e.PackKey);

        if (e.IsDelete)
        {
            if (pack == null)
                return;

            _dataPacks.Remove(e.PackKey);
            pack.Dispose();
            _logger?.LogInformation("Pack '{key}' removed", e.PackKey);
        }
        else
        {
            if (pack != null)
                return;

            var loader = _source.GetLoader(e.PackKey);
            if (loader == null)
                return;

            pack = new DataPack<ContextT>(loader, _mapper, _logger);
            pack.OnDataUpdated += Update;
            pack.Load();
            _dataPacks.Add(e.PackKey, pack);
            _logger?.LogInformation("Pack '{key}' added", e.PackKey);
        }

        if (_combinedPack != null)
        {
            try
            {
                _mapper.CombineProperties()(_dataPacks.Select(p => p.Value.Data), _combinedContext!);
                _logger?.LogInformation("Combined pack updated");
            }
            catch (TargetInvocationException ex)
            {
                _logger?.LogError(ex.InnerException, "Combined pack dont updated");
                return;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Combined pack dont updated");
                return;
            }
        }
    }

    private void Update(object? sender, IDataUpdatedEventArgs e)
    {
        if (_dataPacks == null)
            throw new InvalidOperationException();
        if (_combinedPack == null)
            return;
        try
        {
            _mapper.CombineProperty(e.Key)(_dataPacks.Select(p => p.Value.Data), _combinedContext!);
            _logger?.LogInformation("Combined pack property '{key}' updated", e.Key);
        }
        catch (KeyNotFoundException)
        {
            return;
        }
        catch (TargetInvocationException ex)
        {
            _logger?.LogError(ex.InnerException, "Combined pack property '{key}' dont updated", e.Key);
            return;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Combined pack property '{key}' dont updated", e.Key);
            return;
        }
        OnDataUpdated?.Invoke(this, e);
    }

    private bool disposedValue;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _source.Dispose();
                if (_dataPacks != null)
                    foreach (var pack in _dataPacks)
                        pack.Value.Dispose();
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
