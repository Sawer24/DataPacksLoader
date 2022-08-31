namespace FilePacksLoader.Interfaces;

public interface IDataCombinerFactory
{
    bool CanCombine(Type typeToCombine);

    IDataCombiner CreateCombiner(Type typeToCombine);
}
