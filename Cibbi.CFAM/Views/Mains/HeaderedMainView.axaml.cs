using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Cibbi.CFAM.ViewModels;
using Cibbi.CFAM.ViewModels.Mains;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace Cibbi.CFAM.Views.Mains;

public partial class HeaderedMainView : CFAMUserControl<HeaderedMainViewModel>
{
    private List<Control> _overlays = new();
    private WindowNotificationManager? _notificationManager;
    public HeaderedMainView()
    {
        InitializeComponent();
        this.WhenActivated(disposable =>
        {
            if (ViewModel is null) return;
            ViewModel.Router.NavigationChanged.Subscribe(_ =>
            {
                var x = ViewModel.Router.GetCurrentViewModel();
                if(x is null) return;
                MainContent.Children.Clear();
                if (ViewModel.ViewLocator.ResolveView(x) is not Control control) return;
                control.DataContext = x;
                MainContent.Children.Add(control);
                
                if (ViewModel.Router.NavigationStack.Count > 1)
                    BackButton.Classes.Add("visible");
                else
                    BackButton.Classes.Remove("visible");

            }).DisposeWith(disposable);
            
            if (ViewModel is INotificationsReceiver receiver)
            {
                FlushPendingNotifications(receiver);

                receiver.PendingNotifications.ToObservableChangeSet()
                    .SelectMany(x => x)
                    .Where(x => x.Reason == ListChangeReason.Add)
                    .Select(x => x.Item.Current)
                    .Subscribe(x =>
                    {
                        FlushPendingNotifications(receiver);
                    }).DisposeWith(disposable);
            }

            if (ViewModel is IOverlaysProvider provider)
            {
                provider.Overlays.AsObservableChangeSet()
                    .Where(x => x.Adds > 0 || x.Removes > 0)
                    .Subscribe(x =>
                    {
                        //MainGrid.Children.Remove(Overlay);
                        foreach (var overlay in _overlays)
                        {
                            MainGrid.Children.Remove(overlay);
                        }
                        //Overlay.Children.Clear();
                        if (provider.Overlays.Count <= 0) return;
                        foreach (var overlay in provider.Overlays.OrderBy(x => x.Value.ZIndex))
                        {
                            var overlayView = new OverlayView
                            {
                                DataContext = overlay.Value,
                                [Grid.RowProperty] = 0,
                                [Grid.ColumnProperty] = 0,
                                [Grid.RowSpanProperty] = 2,
                                [Grid.ColumnSpanProperty] = 2
                            };
                            MainGrid.Children.Add(overlayView);
                            _overlays.Add(overlayView);
                        }
                    }).DisposeWith(disposable);
            }
        });
    }
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        
        _notificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(this))
        {
            Position = CFAMSettings.NotificationPosition,
            MaxItems = CFAMSettings.MaxNotifications,
        };
    }
    
    private void FlushPendingNotifications(INotificationsReceiver receiver)
    {
        if(receiver.PendingNotifications.Count == 0) return;
        
        foreach (var x in receiver.PendingNotifications.ToList())
        {
            _notificationManager?.Show(new Avalonia.Controls.Notifications.Notification(
                x.Title,
                x.Message,
                GetNotificationType(x.Type),
                x.Duration));
            
            receiver.PendingNotifications.Remove(x);
        }
    }
    
    private static NotificationType GetNotificationType(NotificationLevel level)
    {
        return level switch
        {
            NotificationLevel.Information => NotificationType.Information,
            NotificationLevel.Success => NotificationType.Success,
            NotificationLevel.Warning => NotificationType.Warning,
            NotificationLevel.Error => NotificationType.Error,
            _ => NotificationType.Information
        };
    }
}