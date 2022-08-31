﻿using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DataPacksLoader.Files;

public static class PacksLoaderFactory
{
    public static IDataPack<ContextT> CreateJsonDataPack<ContextT>(string path, JsonSerializerOptions? options = null, Predicate<string>? folgersFilter = null, ILogger? logger = null) where ContextT : class, new()
    {
        var mapperOptions = new DataPackMapperOptions
        {
            PropertiesPolicy = new FilesPropertiesPolicy(".json")
        };
        return new DataPackBuilder<ContextT>()
            .UseFilesLoader(path, b => b
                .UseSerializer(new JsonDataSerializer(options))
                .UseLogger(logger)
                .UseWatcher())
            .UseMapper(b => b
                .UseOptions(mapperOptions)
                .UseLogger(logger))
            .UseLogger(logger).Build();
    }

    public static IDataPacksCollection<ContextT> CreateJsonDataPacksCollection<ContextT>(string path, JsonSerializerOptions? options = null, Predicate<string>? folgersFilter = null, ILogger? logger = null) where ContextT : class, new()
    {
        var mapperOptions = new DataPackMapperOptions
        {
            PropertiesPolicy = new FilesPropertiesPolicy(".json"),
            Combiner = new DataCombinersCollection
            {
                Factories = new List<IDataCombinerFactory>
                {
                    new CollectionsDataCombinerFactory()
                }
            }
        };
        return new DataPacksCollectionBuilder<ContextT>()
            .UseFilesSource(path, b => b
                .UseSerializer(new JsonDataSerializer(options))
                .AddFilter(folgersFilter)
                .UseLogger(logger)
                .UseWatcher())
            .UseMapper(b => b
                .UseOptions(mapperOptions)
                .UseLogger(logger))
            .UseLogger(logger).Build();
    }
}
