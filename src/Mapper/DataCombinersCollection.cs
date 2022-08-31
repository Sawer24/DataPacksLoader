namespace DataPacksLoader.Mapper;

public class DataCombinersCollection : IDataCombinersCollection
{
    public required IEnumerable<IDataCombinerFactory> Factories { get; init; }

    public IDataCombiner GetCombiner(Type type) =>
        Factories.FirstOrDefault(c => c.CanCombine(type))?.CreateCombiner(type) ??
        throw new CombinerNotFoundException(type);
}
