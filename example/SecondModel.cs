using System.Text.Json.Serialization;

namespace JsonFilesPacksExample;

public class SecondModel
{
    public string? Name { get; set; }
    [JsonPropertyName("Desc")]
    public string? Description { get; set; }
}
