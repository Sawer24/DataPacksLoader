using Microsoft.Extensions.Logging;

namespace DataPacksLoader.Mapper;

public class DataPackMapperBuilder<ContextT>
    where ContextT : class, new()
{
    protected IDataPackMapperOptions? _options;
    protected ILogger? _logger;

    public DataPackMapper<ContextT> Build()
    {
        if (_options == null)
            throw new InvalidOperationException("Options not specified");
        return new DataPackMapper<ContextT>(_options, _logger);
    }

    public DataPackMapperBuilder<ContextT> UseOptions(IDataPackMapperOptions options)
    {
        _options = options;
        return this;
    }

    public DataPackMapperBuilder<ContextT> UseLogger(ILogger? logger)
    {
        _logger = logger;
        return this;
    }
}
