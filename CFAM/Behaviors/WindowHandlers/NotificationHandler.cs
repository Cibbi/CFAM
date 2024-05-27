using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using CFAM.ViewModels.WindowServices;
using DynamicData;
using DynamicData.Binding;

namespace CFAM.Behaviors.WindowHandlers;

public class NotificationHandler : WindowServiceHandler
{
    private Notifications _notifications = new();
    private WindowNotificationManager? _notificationManager;
    private CompositeDisposable? _disposable;
    
    protected override void OnDataContextChanged()
    {
        Provider?.AddService(_notifications);
    }
    protected override void OnAttachedToVisualTree()
    {
        base.OnAttachedToVisualTree();
        
        _disposable = new CompositeDisposable();
        
        _notificationManager = new WindowNotificationManager(TopLevel.GetTopLevel(AssociatedObject))
        {
            Position = CFAMSettings.NotificationPosition,
            MaxItems = CFAMSettings.MaxNotifications,
        };
        
        _notifications.PendingNotifications.ToObservableChangeSet()
            .SelectMany(x => x)
            .Where(x => x.Reason == ListChangeReason.Add)
            .Select(x => x.Item.Current)
            .Subscribe(_ =>
            {
                FlushPendingNotifications();
            }).DisposeWith(_disposable);
    }
    
    protected override void OnDetachedFromVisualTree()
    {
        _disposable?.Dispose();
    }
    
    private void FlushPendingNotifications()
    {
        if(_notifications.PendingNotifications.Count == 0) return;
        
        foreach (var x in _notifications.PendingNotifications.ToList())
        {
            _notificationManager?.Show(new Avalonia.Controls.Notifications.Notification(
                x.Title,
                x.Message,
                GetNotificationType(x.Type),
                x.Duration));
            
            _notifications.PendingNotifications.Remove(x);
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