using System.Reactive;
using Avalonia;
using Cibbi.CFAM.Services;
using ReactiveUI;

namespace Cibbi.CFAM.ViewModels.Windows
{
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        public RoutingState Router { get; } = new ();
        public int NavigationStackCount => Router.NavigationStack.Count;
        
        private readonly PagesProvider _pagesProvider;
        public IViewLocator ViewLocator { get; }

        public MainWindowViewModel()
        {
            _pagesProvider = AvaloniaLocator.Current.GetRequiredService<PagesProvider>();
            ViewLocator = AvaloniaLocator.Current.GetRequiredService<IViewLocator>();
            
            Router.Navigate.ThrownExceptions.Subscribe(_ => {}); //TODO: proper exception handling maybe
            Router.NavigateBack.ThrownExceptions.Subscribe(_ => {});
            Router.NavigateAndReset.ThrownExceptions.Subscribe(_ => {});

            NavigateTo(_pagesProvider.GetPages().First().PageType);
        }

        public void NavigateTo(Type routeType, bool reset = false)
        {
            IRoutableViewModel r;
            var lastRouteOfType = Router.NavigationStack.LastOrDefault(x => x.GetType() == routeType);
            if (lastRouteOfType is not null)
            {
                r = lastRouteOfType;
            }
            else
            {
                var route = (RoutableViewModel) Activator.CreateInstance(routeType, this)!;
                r = route;
            }
            
            
            if(reset)
                Router.NavigateAndReset.Execute(r);
            else
                Router.Navigate.Execute(r);
        }
        
        public void NavigateTo(IRoutableViewModel route, bool reset = false)
        {
            var lastRouteOfType = Router.NavigationStack.LastOrDefault(x => x == route);
            IRoutableViewModel r = lastRouteOfType ?? route;

            if(reset)
                Router.NavigateAndReset.Execute(r);
            else
                Router.Navigate.Execute(r);
        }
        

        public void NavigateBack()
        {
            Router.NavigateBack.Execute(Unit.Default);
        }
        
        public IEnumerable<Page> GetPages()
        {
            return _pagesProvider.GetPages();
        }
    }
}