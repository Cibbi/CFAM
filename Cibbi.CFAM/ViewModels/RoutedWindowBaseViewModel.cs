using ReactiveUI;

namespace Cibbi.CFAM.ViewModels;

public abstract class RoutedWindowBaseViewModel : WindowBaseViewModel, IScreen
{
    public RoutingState Router
    {
        get
        {
            if (WindowRouter is null)
                WindowRouter = new RoutingState();
            return WindowRouter;
        }
        protected set => WindowRouter = value;
    }
        
    public int NavigationStackCount => Router.NavigationStack.Count;
}