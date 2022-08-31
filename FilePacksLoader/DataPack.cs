using Microsoft.Extensions.Logging;
using System.Reflection;

namespace FilePacksLoader;

public class DataPack<ContextT> : IDataPack<ContextT>
    where ContextT : class, new()
{
    protected readonly IDataLoader _loader;
    protected readonly IDataPackMapper<ContextT> _mapper;
    protected readonly ILogger? _logger;

    protected ContextT? _data;

    public ContextT Data => _data ?? throw new InvalidOperationException();

    public event EventHandler<IDataUpdatedEventArgs>? OnDataUpdated;

    public DataPack(IDataLoader loader, IDataPackMapper<ContextT> mapper, ILogger? logger = null)
    {
        _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
    }

    public IDataPack<ContextT> Load()
    {
        if (_data != null)
            throw new InvalidOperationException();
        var context = new ContextT();
        try
        {
            _mapper.LoadProperties()(_loader, context);
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException!;
        }
        _data = context;
        _loader.OnDataUpdated += Update;
        return this;
    }

    public IDataPack<ContextT> Save() => throw new NotSupportedException();

    private void Update(object? sender, IDataUpdatedEventArgs e)
    {
        if (_data == null)
            throw new InvalidOperationException();
        try
        {
            _mapper.LoadProperty(e.Key)(_loader, _data);
            _logger?.LogInformation("Pack property '{key}' updated", e.Key);
        }
        catch (KeyNotFoundException)
        {
            return;
        }
        catch (TargetInvocationException ex)
        {
            _logger?.LogError(ex.InnerException, "Pack property '{key}' dont updated", e.Key);
            return;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.InnerException, "Pack property '{key}' dont updated", e.Key);
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
                _loader.Dispose();
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
