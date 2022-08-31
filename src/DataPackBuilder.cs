using Microsoft.Extensions.Logging;

namespace DataPacksLoader;

public class DataPackBuilder<ContextT>
    where ContextT : class, new()
{
    protected IDataLoader? _loader;
    protected IDataPackMapper<ContextT>? _mapper;
    protected ILogger? _logger;

    public DataPack<ContextT> Build()
    {
        if (_loader == null)
            throw new InvalidOperationException("Loader not specified");
        if (_mapper == null)
            throw new InvalidOperationException("Mapper not specified");
        return new DataPack<ContextT>(_loader, _mapper, _logger);
    }

    public DataPackBuilder<ContextT> UseLoader(IDataLoader loader)
    {
        _loader = loader;
        return this;
    }

    public DataPackBuilder<ContextT> UseMapper(IDataPackMapper<ContextT> mapper)
    {
        _mapper = mapper;
        return this;
    }

    public DataPackBuilder<ContextT> UseLogger(ILogger? logger)
    {
        _logger = logger;
        return this;
    }
}
