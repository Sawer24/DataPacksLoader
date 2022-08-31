using System.Reflection;

namespace DataPacksLoader.Interfaces;

public interface IPropertiesPolicy
{
    IPropertyPolicy GetPropertyPolicy(PropertyInfo property);
}
