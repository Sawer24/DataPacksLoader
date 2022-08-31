namespace DataPacksLoader.Schema;

[AttributeUsage(AttributeTargets.Property)]
public class FileNameAttribute : Attribute
{
    public string Name { get; set; }

    public FileNameAttribute(string name)
    {
        Name = name;
    }
}
