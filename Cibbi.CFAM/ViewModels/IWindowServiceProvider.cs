namespace Cibbi.CFAM.ViewModels;

public interface IWindowServiceProvider
{
    public WindowServiceProvider WindowServices { get; set; }
}

public interface IWindowService
{
    
}

public class WindowServiceProvider
{
    private List<IWindowService> _services = new ();
    
    public event EventHandler<AddedServiceEventArgs>? ServiceAdded;

    public T? GetService<T>() where T : class, IWindowService
    {
        return _services.FirstOrDefault(s => s.GetType() == typeof(T)) as T;
    }
    
    public bool AddService<T>(T service) where T : class, IWindowService
    {
        if (_services.Any(s => s.GetType() == typeof(T)))
        {
            return false;
        }
        _services.Add(service);
        ServiceAdded?.Invoke(this, new AddedServiceEventArgs(_services, service));
        return true;
    }
}

public class AddedServiceEventArgs
{
    public List<IWindowService> Services { get; set; }
    public IWindowService Added { get; set; }
    
    public AddedServiceEventArgs(List<IWindowService> services, IWindowService added)
    {
        Services = services;
        Added = added;
    }
    
}