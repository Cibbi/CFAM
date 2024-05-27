using ReactiveUI;

namespace CFAM.ViewModels;

public interface IHostScreenProvider
{ 
    IScreen HostScreen { get; }
}

public static class HostScreenProviderExtensions
{
    public static void FetchServiceWhenAvailable<T>(this IHostScreenProvider screenProvider, Action<T> setServiceAction)
    {
        if (screenProvider.HostScreen is not IWindowServiceProvider p)
            throw new Exception("The main screen is not a Windows service provider");


        p.WindowServices.ServiceAdded += WindowServicesOnServiceAdded;
        return;

        void WindowServicesOnServiceAdded(object? sender, AddedServiceEventArgs e)
        {
            if (e.Added is not T s)
                return;

            setServiceAction(s);
            p.WindowServices.ServiceAdded -= WindowServicesOnServiceAdded;
        }
    }
}