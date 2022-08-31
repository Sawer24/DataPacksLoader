using System.Text.Json;

namespace DataPacksLoader.Serializers;

public class JsonDataSerializer : IDataSerializer
{
    private readonly JsonSerializerOptions? _options;

    public JsonDataSerializer(JsonSerializerOptions? options = null)
    {
        _options = options;
    }

    public T? Deserialize<T>(Stream stream) => JsonSerializer.Deserialize<T>(stream, _options);

    public void Serialize(Stream stream, object? value) => JsonSerializer.Serialize(stream, value, _options);
}
