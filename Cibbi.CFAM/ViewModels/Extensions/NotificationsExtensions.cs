namespace Cibbi.CFAM.ViewModels.Extensions;

public static class NotificationsExtensions
{
    public static bool SendNotification(this WindowBaseViewModel receiver, Notification notification)
    {
        if (receiver is not INotificationsReceiver notificationsReceiver)
            return false;
        
        notificationsReceiver.PendingNotifications.Add(notification);
        return true;
    }
    
    public static void SendNotification(this INotificationsReceiver receiver, Notification notification)
    {
        receiver.PendingNotifications.Add(notification);
    }
}