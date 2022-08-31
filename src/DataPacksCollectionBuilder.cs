using Microsoft.Extensions.Logging;

namespace DataPacksLoader;

public class DataPacksCollectionBuilder<ContextT>
    where ContextT : class, new()
{
    protected IDataPacksSource? _source;
    protected IDataPackMapper<ContextT>? _mapper;
    protected ILogger? _logger;

    public DataPacksCollection<ContextT> Build()
    {
        if (_source == null)
            throw new InvalidOperationException("Packs source not specified");
        if (_mapper == null)
            throw new InvalidOperationException("Mapper not specified");
        return new DataPacksCollection<ContextT> (_source, _mapper, _logger);
    }

    public DataPacksCollectionBuilder<ContextT> UsePacksSource(IDataPacksSource source)
    {
        _source = source;
        return this;
    }

    public DataPacksCollectionBuilder<ContextT> UseMapper(IDataPackMapper<ContextT> mapper)
    {
        _mapper = mapper;
        return this;
    }

    public DataPacksCollectionBuilder<ContextT> UseLogger(ILogger? logger)
    {
        _logger = logger;
        return this;
    }
}
