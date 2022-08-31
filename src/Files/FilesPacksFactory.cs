using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DataPacksLoader.Files;

public static class PacksLoaderFactory
{
    public static IDataPack<ContextT> CreateJsonDataPack<ContextT>(string path, FilesPropertiesPolicy policy, IDataSerializer serializer, ILogger? logger = null) where ContextT : class, new()
    {
        var mapperOptions = new DataPackMapperOptions
        {
            PropertiesPolicy = policy
        };
        return new DataPackBuilder<ContextT>()
            .UseFilesLoader(path, b => b
                .UseSerializer(serializer)
                .UseLogger(logger)
                .UseWatcher())
            .UseMapper(b => b
                .UseOptions(mapperOptions)
                .UseLogger(logger))
            .UseLogger(logger).Build();
    }

    public static IDataPack<ContextT> CreateJsonDataPack<ContextT>(string path, FilesPropertiesPolicy policy, JsonSerializerOptions? options = null, ILogger? logger = null) where ContextT : class, new()
    {
        return CreateJsonDataPack<ContextT>(path, policy, new JsonDataSerializer(options), logger);
    }

    public static IDataPack<ContextT> CreateJsonDataPack<ContextT>(string path, IDataSerializer serializer, ILogger? logger = null) where ContextT : class, new()
    {
        return CreateJsonDataPack<ContextT>(path, new FilesPropertiesPolicy(".json"), serializer, logger);
    }

    public static IDataPack<ContextT> CreateJsonDataPack<ContextT>(string path, JsonSerializerOptions? options = null, ILogger? logger = null) where ContextT : class, new()
    {
        return CreateJsonDataPack<ContextT>(path, new FilesPropertiesPolicy(".json"), new JsonDataSerializer(options), logger);
    }

    public static IDataPacksCollection<ContextT> CreateJsonDataPacksCollection<ContextT>(string path, FilesPropertiesPolicy policy, IDataSerializer serializer, ILogger? logger = null) where ContextT : class, new()
    {
        var mapperOptions = new DataPackMapperOptions
        {
            PropertiesPolicy = policy,
            Combiner = new DataCombinersCollection
            {
                Factories = new List<IDataCombinerFactory>
                {
                    new CollectionsDataCombinerFactory(),
                    new DictionariesDataCombinerFactory()
                }
            }
        };
        return new DataPacksCollectionBuilder<ContextT>()
            .UseFilesSource(path, b => b
                .UseSerializer(serializer)
                .UseLogger(logger)
                .UseWatcher())
            .UseMapper(b => b
                .UseOptions(mapperOptions)
                .UseLogger(logger))
            .UseLogger(logger).Build();
    }

    public static IDataPacksCollection<ContextT> CreateJsonDataPacksCollection<ContextT>(string path, FilesPropertiesPolicy policy, JsonSerializerOptions? options = null, ILogger? logger = null) where ContextT : class, new()
    {
        return CreateJsonDataPacksCollection<ContextT>(path, policy, new JsonDataSerializer(options), logger);
    }

    public static IDataPacksCollection<ContextT> CreateJsonDataPacksCollection<ContextT>(string path, IDataSerializer serializer, ILogger? logger = null) where ContextT : class, new()
    {
        return CreateJsonDataPacksCollection<ContextT>(path, new FilesPropertiesPolicy(".json"), serializer, logger);
    }

    public static IDataPacksCollection<ContextT> CreateJsonDataPacksCollection<ContextT>(string path, JsonSerializerOptions? options = null, ILogger? logger = null) where ContextT : class, new()
    {
        return CreateJsonDataPacksCollection<ContextT>(path, new FilesPropertiesPolicy(".json"), new JsonDataSerializer(options), logger);
    }
}
