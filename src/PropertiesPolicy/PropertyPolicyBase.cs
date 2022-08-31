namespace DataPacksLoader.PropertiesPolicy;

public class PropertyPolicyBase : IPropertyPolicy
{
    public bool IsLoad { get; set; }
    public bool IsCombine { get; set; }
    public bool IsRequired { get; set; }
}
