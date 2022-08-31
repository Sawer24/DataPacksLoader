namespace DataPacksLoader.Mapper;

public class DataPackMapperOptions : IDataPackMapperOptions
{
    public required IPropertiesPolicy PropertiesPolicy { get; init; }

    public IDataCombinersCollection? Combiner { get; init; }
}
