namespace FilePacksLoader.Interfaces;

public interface IDataCombinersCollection
{
    /// <exception cref="CombinerNotFoundException"></exception>
    IDataCombiner GetCombiner(Type typeToCombine);
}
