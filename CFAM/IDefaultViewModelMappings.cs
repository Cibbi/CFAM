namespace CFAM;

public interface IDefaultViewModelMappings
{
    bool TryGetMappingFor<T>(out Type type);
    bool TryGetMappingFor(Type vmType, out Type type);
}