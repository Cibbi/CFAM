namespace CFAM.ViewModels.WindowServices;

public partial class Clipboard : IWindowService
{
    public Task<string?> GetClipboardTextAsync()
    {
        if (GetClipboard is null)
            throw new Exception("Clipboard not available");
        return GetClipboard.Invoke();
    }

    public Task SetClipboardTextAsync(string text)
    {
        if (SetClipboard is null)
            throw new Exception("Clipboard not available");
        
        return SetClipboard.Invoke(text);
    }
    internal Func<Task<string?>>? GetClipboard { get; set; }
    internal Func<string, Task>? SetClipboard { get; set; }
}