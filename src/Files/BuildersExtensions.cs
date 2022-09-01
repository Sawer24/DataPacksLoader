namespace DataPacksLoader.Files;

public static class DataPackBuilderExtensions
{
    public static DataPackBuilder<ContextT> UseFilesLoader<ContextT>(this DataPackBuilder<ContextT> packBuilder, string path, Action<FilesDataLoaderBuilder>? action = null)
        where ContextT : class, new()
    {
        var builder = new FilesDataLoaderBuilder(path);
        action?.Invoke(builder);
        packBuilder.UseLoader(builder.Build());

        return packBuilder;
    }
}

public static class DataPacksCollectionBuilderExtensions
{
    public static DataPacksCollectionBuilder<ContextT> UseFilesSource<ContextT>(this DataPacksCollectionBuilder<ContextT> collectionBuilder, string path, Action<FilesPacksSourceBuilder>? action = null)
        where ContextT : class, new()
    {
        var builder = new FilesPacksSourceBuilder(path);
        action?.Invoke(builder);
        collectionBuilder.UsePacksSource(builder.Build());

        return collectionBuilder;
    }
}

public static class DataPackMapperOptionsBuilderExtensions
{
    public static DataPackMapperOptionsBuilder UseFilesPolicy(this DataPackMapperOptionsBuilder optionsBuilder, string filesExtension)
    {
        optionsBuilder.UsePropertiesPolicy(new FilesPropertiesPolicy(filesExtension));
        return optionsBuilder;
    }
    public static DataPackMapperOptionsBuilder UseTextFilesPolicy(this DataPackMapperOptionsBuilder optionsBuilder) => UseFilesPolicy(optionsBuilder, ".txt");
    public static DataPackMapperOptionsBuilder UseJsontFilesPolicy(this DataPackMapperOptionsBuilder optionsBuilder) => UseFilesPolicy(optionsBuilder, ".json");
}

public static class DataPackMapperBuilderExtensions
{
    public static DataPackMapperBuilder<ContextT> UseDefaultFilesOptions<ContextT>(this DataPackMapperBuilder<ContextT> mapperBuilder, string filesExtension) where ContextT : class, new()
    {
        mapperBuilder.UseOptions(new DataPackMapperOptionsBuilder()
            .UseFilesPolicy(filesExtension)
            .UseDefaultCombiner()
            .Build());
        return mapperBuilder;
    }

    public static DataPackMapperBuilder<ContextT> UseDefaultTextFilesOptions<ContextT>(this DataPackMapperBuilder<ContextT> mapperBuilder) where ContextT : class, new()
        => UseDefaultFilesOptions(mapperBuilder, ".txt");

    public static DataPackMapperBuilder<ContextT> UseDefaultJsonFilesOptions<ContextT>(this DataPackMapperBuilder<ContextT> mapperBuilder) where ContextT : class, new()
        => UseDefaultFilesOptions(mapperBuilder, ".json");
}