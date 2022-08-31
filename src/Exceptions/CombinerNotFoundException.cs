namespace DataPacksLoader.Exceptions;

public class CombinerNotFoundException : Exception
{
    public Type Type { get; }

    public CombinerNotFoundException(Type type) : base($"Combiner for type '{type.FullName}' not found")
    {
        Type = type;
    }
}
