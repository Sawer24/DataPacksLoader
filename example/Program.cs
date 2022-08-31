using DataPacksLoader.Files;
using JsonFilesPacksExample;
using Microsoft.Extensions.Logging;
using System.Text.Json;

var logger = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Trace).AddConsole()).CreateLogger(nameof(DataPacksLoader.DataPacksCollection<DataContext>));

var loader = PacksLoaderFactory.CreateJsonDataPacksCollection<DataContext>("exampleData", logger: logger)
    .Load()
    .Combine();

while (true)
{
    Console.WriteLine(JsonSerializer.Serialize(loader, new JsonSerializerOptions { WriteIndented = true }));
    Console.ReadLine();
}