namespace FilePacksLoader.Interfaces.Serializers;

public interface IDataSerializer
{
    T? Deserialize<T>(Stream stream);

    void Serialize(Stream stream, object? value);
}
