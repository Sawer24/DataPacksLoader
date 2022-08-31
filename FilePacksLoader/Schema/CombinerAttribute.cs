namespace FilePacksLoader.Schema;

[AttributeUsage(AttributeTargets.Property)]
public class CombinerAttribute : Attribute
{
    public Type CombinerType { get; }

    public CombinerAttribute(Type combinerType)
    {
        CombinerType = combinerType;
    }
}
