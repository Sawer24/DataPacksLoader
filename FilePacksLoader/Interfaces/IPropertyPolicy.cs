namespace FilePacksLoader.Interfaces;

public interface IPropertyPolicy
{
    bool IsLoad { get; }
    bool IsCombine { get; }
    bool IsRequired { get; }
}
