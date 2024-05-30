using System.Reflection;
using CFAM.Attributes.AutoControl;

namespace CFAM;

public class DefaultViewModelMappings : IDefaultViewModelMappings
{
    private Dictionary<Type, Type> _mappings;

    public DefaultViewModelMappings()
    {
        _mappings = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Select(x => new {Type = x, Attribute = x.GetCustomAttribute<ControlForAttribute>()})
            .Where(x => x.Attribute is not null)
            .ToDictionary(x => x.Attribute!.PropertyType, x => x.Type);
    }
    
    public bool TryGetMappingFor<T>(out Type type)
    {
        Type tType = typeof(T);
        return TryGetMappingFor(tType, out type);
    }
    
    public bool TryGetMappingFor(Type vmType, out Type type)
    {
        return _mappings.TryGetValue(vmType, out type!);
    }
}