using PropertyChanged.SourceGenerator;
using ReactiveUI;
using IViewLocator = Cibbi.CFAM.Services.IViewLocator;


namespace Cibbi.CFAM.ViewModels;

public abstract class RoutedWindowBaseViewModel : WindowBaseViewModel, IScreen
{
    public RoutedWindowBaseViewModel(string title, IViewLocator viewLocator) : this(viewLocator)
    {
        WindowName = title;
    }
    public RoutedWindowBaseViewModel(IViewLocator viewLocator)
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