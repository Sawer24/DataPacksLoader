using FilePacksLoader.Files;
using JsonFilesPackTest;
using Microsoft.Extensions.Logging;
using System.Text.Json;

var logger = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Trace).AddConsole()).CreateLogger(nameof(Program));

var loader = PacksLoaderFactory.CreateJsonDataPacksCollection<TestDataContext>("test", logger: logger).Load().Combine();

while (true)
{
    foreach (var context in loader.DataPacks)
        Console.WriteLine(JsonSerializer.Serialize(context, new JsonSerializerOptions { }));
    Console.WriteLine(JsonSerializer.Serialize(loader.CombinedPack.Data, new JsonSerializerOptions { }));
    Console.ReadLine();
}
