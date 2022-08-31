namespace DataPacksLoader.Mapper;

public class DataPackMapperOptionsBuilder
{
    protected IPropertiesPolicy? _propertiesPolicy;

    protected IDataCombinersCollection? _combiner;

    public DataPackMapperOptions Build()
    {
        if (_propertiesPolicy == null)
            throw new InvalidOperationException("PropertiesPolicy not specified");
        return new DataPackMapperOptions { PropertiesPolicy = _propertiesPolicy, Combiner = _combiner };
    }

    public DataPackMapperOptionsBuilder UsePropertiesPolicy(IPropertiesPolicy policy)
    {
        _propertiesPolicy = policy;
        return this;
    }

    public DataPackMapperOptionsBuilder UseCombiner(IDataCombinersCollection? combiner)
    {
        _combiner = combiner;
        return this;
    }
}
