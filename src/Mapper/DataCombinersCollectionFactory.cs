namespace DataPacksLoader.Mapper;

public static class DataCombinersCollectionFactory
{
    public static DataCombinersCollection CreateDefault(Action<DataCombinersCollectionBuilder>? action = null)
    {
        var builder = new DataCombinersCollectionBuilder();
        builder.AddFactory(new CollectionsDataCombinerFactory());
        action?.Invoke(builder);
        return builder.Build();
    }
}
