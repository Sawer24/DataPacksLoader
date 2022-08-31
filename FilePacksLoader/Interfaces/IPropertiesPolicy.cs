using System.Reflection;

namespace FilePacksLoader.Interfaces;

public interface IPropertiesPolicy
{
    IPropertyPolicy GetPropertyPolicy(PropertyInfo property);
}
