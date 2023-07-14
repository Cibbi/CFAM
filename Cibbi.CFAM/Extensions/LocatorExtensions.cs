using Splat;

namespace Cibbi.CFAM.Extensions;

public static class LocatorExtensions
{
    public static T GetRequiredService<T>(this IReadonlyDependencyResolver resolver, string? contract = null)
    {
        var result = resolver.GetService<T>(contract);
        
        if(result is null)
            throw new NullReferenceException($"The service for intrefact {nameof(T)} was not found");

        return result;
    }
    
    public static IMutableDependencyResolver RegisterLazySingletonAnd<TAs, T>(this IMutableDependencyResolver resolver, string? contract = null)
        where T : TAs, new()
    {
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        var val = new Lazy<object>(() => new T(), LazyThreadSafetyMode.ExecutionAndPublication);
        resolver.Register(() => val.Value, typeof(TAs), contract);
        return resolver;
    }
    
    public static IMutableDependencyResolver RegisterSingletonAnd<TAs, T>(this IMutableDependencyResolver resolver, string? contract = null)
        where T : TAs, new()
    {
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        var val =  new T();
        resolver.Register(() => val, typeof(TAs), contract);
        return resolver;
    }
    
    public static IMutableDependencyResolver RegisterSingletonAnd<T>(this IMutableDependencyResolver resolver, string? contract = null)
        where T : new()
    {
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        var val =  new T();
        resolver.Register(() => val, typeof(T), contract);
        return resolver;
    }
}