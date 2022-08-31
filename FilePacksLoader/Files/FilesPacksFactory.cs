using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FilePacksLoader.Files;

public static class PacksLoaderFactory
{
    public static IDataPack<ContextT> CreateJsonDataPack<ContextT>(string path, FilesPropertiesPolicy policy, IDataSerializer serializer, ILogger? logger = null) where ContextT : class, new()
    {
        var options = new DataPackMapperOptions
        {
            PropertiesPolicy = policy
        };
        var mapper = new DataPackMapper<ContextT>(options, logger);
        mapper.Map(false);
        return new DataPack<ContextT>(new FilesDataLoader(path, serializer, logger).UseWatcher(), mapper, logger);
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
        var options = new DataPackMapperOptions
        {
            PropertiesPolicy = policy,
            Combiner = new DataCombinersCollection
            {
                Factories = new List<IDataCombinerFactory>
                {
                    new CollectionsDataCombinerFactory()
                }
            }
        };
        var mapper = new DataPackMapper<ContextT>(options, logger);
        mapper.Map(true);
        return new DataPacksCollection<ContextT>(new FilesPacksSource<ContextT>(path, serializer, logger).UseWatcher(), mapper, logger);
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
