namespace FilePacksLoader.Interfaces;

public interface IDataPackMapperOptions
{
    IPropertiesPolicy PropertiesPolicy { get; }

    IDataCombinersCollection? Combiner { get; }
}
