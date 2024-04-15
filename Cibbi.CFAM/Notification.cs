namespace Cibbi.CFAM;

public class Notification
{
    public string Title { get; set; }
    public string Message { get; set; }
    public NotificationLevel Type { get; set; }
    public TimeSpan Duration { get; set; }
    
    public Notification(string title, string message, NotificationLevel type, TimeSpan duration)
    {
        Title = title;
        Message = message;
        Type = type;
        Duration = duration;
    }
    
    public Notification(string title, string message, NotificationLevel type = NotificationLevel.Information) : this(title, message, type, TimeSpan.FromSeconds(5)) { }
    public Notification(string message) : this(string.Empty, message) { }
}