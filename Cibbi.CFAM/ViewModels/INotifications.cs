using System.Collections.ObjectModel;

namespace Cibbi.CFAM.ViewModels;

public interface INotificationsSender
{
    public ObservableCollection<Notification> PendingNotifications { get; }
}