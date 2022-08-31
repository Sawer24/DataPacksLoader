using FilePacksLoader.Schema;

namespace JsonFilesPackTest;

public class TestDataContext
{
    //[Required]
    [NotCombined]
    [FileName("desc")]
    public DataPackDescription? Description { get; set; }

    [Required]
    [FileName("models")]
    public List<TestModel>? TestModels { get; set; }
}
