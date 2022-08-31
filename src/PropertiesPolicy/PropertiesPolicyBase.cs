using System.Reflection;

namespace DataPacksLoader.PropertiesPolicy;

public class PropertiesPolicyBase : IPropertiesPolicy
{
    public IPropertyPolicy GetPropertyPolicy(PropertyInfo property)
    {
        var policy = CreatePolicy(property);
        policy.IsLoad = !property.GetCustomAttributes(typeof(NotMappedAttribute), false).Any();
        policy.IsCombine = !property.GetCustomAttributes(typeof(NotCombinedAttribute), false).Any();
        policy.IsRequired = property.GetCustomAttributes(typeof(RequiredAttribute), false).Any();
        return policy;
    }

    protected virtual PropertyPolicyBase CreatePolicy(PropertyInfo property) => new PropertyPolicyBase();
}
