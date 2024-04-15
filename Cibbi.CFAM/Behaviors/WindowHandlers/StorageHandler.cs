using Avalonia.Controls;
using Cibbi.CFAM.ViewModels.WindowServices;

namespace Cibbi.CFAM.Behaviors.WindowHandlers;

public class StorageHandler : WindowServiceHandler
{
    private Storage _storage;

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