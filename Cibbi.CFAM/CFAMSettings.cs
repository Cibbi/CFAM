using Avalonia.Controls.Notifications;

namespace Cibbi.CFAM;

public static class CFAMSettings
{
    public static NotificationPosition NotificationPosition { get; set; } = NotificationPosition.BottomRight;
    public static int MaxNotifications { get; set; } = 5;
}