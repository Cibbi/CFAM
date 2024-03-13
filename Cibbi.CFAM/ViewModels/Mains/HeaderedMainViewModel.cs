using System.Collections.ObjectModel;
using System.Reactive;
using Cibbi.CFAM.ViewModels.Dialogs;
using ReactiveUI;
using IViewLocator = Cibbi.CFAM.Services.IViewLocator;

namespace Cibbi.CFAM.ViewModels.Mains;

public partial class HeaderedMainViewModel : RoutedWindowBaseViewModel, IOverlaysProvider, 
    IDialogProvider, INotificationsReceiver, IClipboardProvider
{
    public Clipboard Clipboard { get; } = new();
    public Dictionary<string, OverlayViewModel> Overlays { get; } = new();
    public ObservableCollection<Notification> PendingNotifications { get; } = new();
    
    public DialogsOverlayViewModel DialogsOverlay { get; } = new();
    public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }
    public HeaderedMainViewModel(string title, IViewLocator viewLocator) : base(title, viewLocator)
    {
        NavigateBackCommand = ReactiveCommand.Create(NavigateBack);
        HasCustomTitleBar = true;
        
        Overlays.Add("Dialogs", new OverlayViewModel(DialogsOverlay){ IsEnabled = true, ZIndex = 100});
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
            Router.NavigateAndReset.Execute(r).Subscribe();
        else
            Router.Navigate.Execute(r).Subscribe();
    }
        
    public void NavigateTo(IRoutableViewModel route, bool reset = false)
    {
        var lastRouteOfType = Router.NavigationStack.LastOrDefault(x => x == route);
        IRoutableViewModel r = lastRouteOfType ?? route;

        if(reset)
            Router.NavigateAndReset.Execute(r).Subscribe();
        else
            Router.Navigate.Execute(r).Subscribe();
    }

    public void NavigateBack()
    {
        Router.NavigateBack.Execute(Unit.Default).Subscribe();
    }
}