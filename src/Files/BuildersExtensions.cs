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