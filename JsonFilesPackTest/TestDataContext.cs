using FilePacksLoader.Schema;

namespace JsonFilesPackTest;

public class TestDataContext
{
    [Required]
    [NotCombined]
    [PropertyName("Desc")]
    public DataPackDescription? Description { get; protected set; }

    public List<TestModel>? TestModels { get; protected set; }
}
