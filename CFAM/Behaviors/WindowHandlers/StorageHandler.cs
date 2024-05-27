using Avalonia.Controls;
using CFAM.ViewModels.WindowServices;

namespace CFAM.Behaviors.WindowHandlers;

public class StorageHandler : WindowServiceHandler
{
    private readonly Storage _storage;

    public StorageHandler()
    {
        _storage = new Storage();
    }
    
    protected override void OnDataContextChanged()
    {
        _storage.StorageProvider = TopLevel.GetTopLevel(AssociatedObject)?.StorageProvider;
        Provider?.AddService(_storage);
        
    }
}