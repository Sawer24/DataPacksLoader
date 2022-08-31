namespace DataPacksLoader.Mapper;

public static class DataPackBuilderExtensions
{
    public static DataPackBuilder<ContextT> UseMapper<ContextT>(this DataPackBuilder<ContextT> packBuilder, Action<DataPackMapperBuilder<ContextT>>? action = null)
        where ContextT : class, new()
    {
        var builder = new DataPackMapperBuilder<ContextT>();
        action?.Invoke(builder);
        packBuilder.UseMapper(builder.Build().Map(false));

        return packBuilder;
    }
}

public static class DataPacksCollectionBuilderExtensions
{
    public static DataPacksCollectionBuilder<ContextT> UseMapper<ContextT>(this DataPacksCollectionBuilder<ContextT> collectionBuilder, Action<DataPackMapperBuilder<ContextT>>? action = null)
        where ContextT : class, new()
    {
        var builder = new DataPackMapperBuilder<ContextT>();
        action?.Invoke(builder);
        collectionBuilder.UseMapper(builder.Build().Map(true));

        return collectionBuilder;
    }
}