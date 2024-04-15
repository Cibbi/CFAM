using System.Collections.ObjectModel;

namespace Cibbi.CFAM.ViewModels.WindowServices;

public class Notifications : IWindowService
{
    internal ObservableCollection<Notification> PendingNotifications { get; } = [];

    public void SendNotification(string title, string message, NotificationLevel type, TimeSpan duration)
    {
        PendingNotifications.Add(new Notification(title, message, type, duration));
    }

    public void SendNotification(string title, string message, NotificationLevel type = NotificationLevel.Information)
    {
        PendingNotifications.Add(new Notification(title, message, type));
    }
}