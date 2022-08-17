using FilePacksLoader.Files;
using JsonFilesPackTest;
using Microsoft.Extensions.Logging;
using System.Text.Json;

var logger = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Trace).AddConsole()).CreateLogger(nameof(Program));

var loader = new FilesPacksCollection<TestDataContext>("test", logger);
loader.UseWatcher();
loader.Load();

var pack = loader.Combine();

//TestDataContext context = (TestDataContext)loader.Combine();

while (true)
{
    foreach (var context in loader.DataPacks)
        Console.WriteLine(JsonSerializer.Serialize(context, new JsonSerializerOptions { }));
    Console.ReadLine();
}