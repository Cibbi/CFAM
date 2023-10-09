using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Cibbi.CFAM.ViewModels;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using NotificationType = Avalonia.Controls.Notifications.NotificationType;

namespace Cibbi.CFAM.Views;

public class CFAMNotifiedUserControl<T> : CFAMUserControl<T> where T : ViewModelBase
{

    private WindowNotificationManager? _notificationManager;
    public CFAMNotifiedUserControl()
    {
        this.WhenActivated(disposable =>
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (ViewModel is INotificationsSender sender)
            {
                sender.PendingNotifications.ToObservableChangeSet()
                    .SelectMany(x => x)
                    .Where(x => x.Reason == ListChangeReason.Add)
                    .Select(x => x.Item.Current)
                    .Subscribe(x =>
                    {
                        
                            _notificationManager?.Show(new Avalonia.Controls.Notifications.Notification(
                                x.Title,
                                x.Message,
                                GetNotificationType(x.Type),
                                x.Duration));

                            sender.PendingNotifications.Remove(x);
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