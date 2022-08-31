namespace FilePacksLoader.Mapper;

public abstract class DataCombiner<T> : IDataCombinerFactory, IDataCombiner
{
    public virtual bool CanCombine(Type typeToCombine) => typeof(T) == typeToCombine;

    public IDataCombiner CreateCombiner(Type typeToCombine) => this;

    public abstract object? Combine(IEnumerable<object?> values, IPropertyPolicy policy);

}
