using Avalonia.Platform.Storage;

namespace CFAM.ViewModels.WindowServices;

public class Storage : IWindowService
{
    public IStorageProvider? StorageProvider { get; internal set; }
}