using System.Reflection;

namespace FilePacksLoader.Files;

public sealed class FilesPropertiesPolicy : PropertiesPolicyBase
{
    private readonly string _fileExtension;

    public FilesPropertiesPolicy(string fileExtension)
    {
        _fileExtension = fileExtension;
    }

    protected override PropertyPolicyBase CreatePolicy(PropertyInfo property)
    {
        return new FilesPropertyPolicy
        {
            FilePath = (((FileNameAttribute?)property.GetCustomAttributes(typeof(FileNameAttribute), false).FirstOrDefault())?.Name ?? property.Name) + _fileExtension,
        };
    }
}
