using ReactiveUI;

namespace Cibbi.CFAM.ViewModels;

public class RoutedBaseViewModel : IScreen
{
    public RoutedBaseViewModel(IViewLocator viewLocator)
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
    
    public IViewLocator ViewLocator { get; }
}