using DataPacksLoader.Schema;

namespace JsonFilesPacksExample;

public class DataContext
{
    [Required]
    [NotCombined]
    [FileName("desc")]
    public DataPackDescription? Description { get; set; }

    [Required]
    [FileName("models")]
    public List<Model>? List { get; set; }

    [FileName("modelsDict")]
    public Dictionary<int, SecondModel>? Dictionary { get; set; }

    public Version Version => Description!.Version;
}
