namespace DataPacksLoader.Mapper;

public class DataCombinersCollectionBuilder
{
    protected List<IDataCombinerFactory> _factories = new();

    public DataCombinersCollection Build()
    {
        return new DataCombinersCollection { Factories = _factories };
    }

    public DataCombinersCollectionBuilder AddFactory(IDataCombinerFactory factory)
    {
        _factories.Add(factory);
        return this;
    }
}
