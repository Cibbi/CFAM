using System.Collections.ObjectModel;

namespace Cibbi.CFAM.ViewModels;

public interface INotificationsReceiver
{
    public ObservableCollection<Notification> PendingNotifications { get; }
}