using System.Reactive;
using Cibbi.CFAM.Extensions;
using Cibbi.CFAM.Services;
using Cibbi.CFAM.ViewModels;
using PropertyChanged.SourceGenerator;
using ReactiveUI;
using Splat;

namespace Cibbi.CFAM.FluentAvalonia.ViewModels.Windows
{
    public partial class MainFluentWindowViewModel : RoutedWindowBaseViewModel
    {
        [Notify] private double _closedPaneWidth = 48.0;
        [Notify] private double _openPaneWidth = 320.0;
        [Notify] private bool _isPaneToggleVisible;
        [Notify] private PanePosition _panePosition = PanePosition.Left;
        [Notify] private PaneState _paneState = PaneState.Compact;

        [Notify] private string _mainListing = "";
        [Notify] private string? _optionsListing;

        private readonly IPagesProvider _pagesProvider;

        public MainFluentWindowViewModel()
        {
            _pagesProvider =Locator.Current.GetRequiredService<IPagesProvider>();

            Router.Navigate.ThrownExceptions.Subscribe(_ => {}); //TODO: proper exception handling maybe
            Router.NavigateBack.ThrownExceptions.Subscribe(_ => {});
            Router.NavigateAndReset.ThrownExceptions.Subscribe(_ => {});
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
        
        public IEnumerable<Page> GetPages(string listing = "")
        {
            return _pagesProvider.GetPages(listing);
        }
    }
}