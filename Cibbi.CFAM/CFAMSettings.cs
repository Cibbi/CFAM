using Avalonia.Controls.Notifications;
using Cibbi.CFAM.ViewModels;
using ReactiveUI;

namespace Cibbi.CFAM;

public static class CFAMSettings
{
    public static NotificationPosition NotificationPosition { get; set; } = NotificationPosition.BottomRight;
    public static int MaxNotifications { get; set; } = 5;
    
    public static IViewLocator ViewLocator { get; set; } = new BaseViewLocator();
}