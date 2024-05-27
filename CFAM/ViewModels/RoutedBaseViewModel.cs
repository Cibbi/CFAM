using ReactiveUI;
using Services_IViewLocator = CFAM.Services.IViewLocator;

namespace CFAM.ViewModels;

public class RoutedBaseViewModel : IScreen
{
    public RoutedBaseViewModel(Services_IViewLocator viewLocator)
    {
        ViewLocator = viewLocator;
    }
    public RoutingState Router
    {
        get => _windowRouter ??= new RoutingState();
        protected set => _windowRouter = value;
    }

    private RoutingState? _windowRouter;
        
    public int NavigationStackCount => Router.NavigationStack.Count;
    
    public Services_IViewLocator ViewLocator { get; }
}