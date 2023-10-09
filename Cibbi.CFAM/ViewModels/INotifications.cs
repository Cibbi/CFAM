using System.Collections.ObjectModel;

namespace Cibbi.CFAM.ViewModels;

public interface INotificationsSender
{
    public ObservableCollection<Notification> PendingNotifications { get; }
}

public class Notification
{
    public string Title { get; set; }
    public string Message { get; set; }
    public NotificationLevel Type { get; set; }
    public TimeSpan Duration { get; set; }
}

public enum NotificationLevel
{
    Information,
    Success,
    Warning,
    Error
}