using System.Collections.ObjectModel;

namespace CFAM.ViewModels;

public interface INotificationsReceiver
{
    public ObservableCollection<Notification> PendingNotifications { get; }
}