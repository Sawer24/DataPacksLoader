using DataPacksLoader.Files;
using JsonFilesPacksExample;
using Microsoft.Extensions.Logging;
using System.Text.Json;

var logger = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Trace).AddConsole()).CreateLogger(nameof(DataPacksLoader.DataPacksCollection<DataContext>));

var loader = FilesPacksFactory.CreateJsonDataPacksCollection<DataContext>("exampleData",
    folgersFilter: p => File.Exists(p + "/desc.json"),
    logger: logger)
    .Load()
    .Combine();

while (true)
{
    Console.WriteLine(JsonSerializer.Serialize(loader, new JsonSerializerOptions { WriteIndented = true }));
    Console.ReadLine();
}