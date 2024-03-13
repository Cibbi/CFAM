using Avalonia.Controls.Notifications;
using Cibbi.CFAM.Services;
using Cibbi.CFAM.ViewModels;

namespace Cibbi.CFAM;

public static class CFAMSettings
{
    public static NotificationPosition NotificationPosition { get; set; } = NotificationPosition.BottomRight;
    public static int MaxNotifications { get; set; } = 5;
    public static double UIScaling { get; set; } = 1;
    
    public static IViewLocator ViewLocator { get; set; } = new BaseViewLocator();
}