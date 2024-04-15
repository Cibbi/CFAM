using ReactiveUI;

namespace Cibbi.CFAM.ViewModels;

public abstract class RoutableViewModel : ViewModelBase, IRoutableViewModel, IHostScreenProvider
{
    public abstract string UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    
    public RoutableViewModel(IScreen screen)
    {
        HostScreen = screen;
    }
}