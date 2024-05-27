using CFAM.ViewModels;
using ReactiveUI;

namespace CFAM.Examples.ViewModels;

public class MainViewModel : RoutableViewModel, IActivatableViewModel
{
    public MainViewModel(IScreen screen) : base(screen)
    {
    }

    public override string UrlPathSegment { get; } = "Home";
    public ViewModelActivator Activator { get; } = new();
}