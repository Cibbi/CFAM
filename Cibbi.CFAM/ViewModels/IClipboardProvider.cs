using PropertyChanged.SourceGenerator;

namespace Cibbi.CFAM.ViewModels;

public interface IClipboardProvider
{
    Clipboard Clipboard { get; }
}

public partial class Clipboard
{
    [Notify] private string _clipboardContentToSend = string.Empty;
    
    public Func<Task<string?>>? GetClipboard { get; set; }
}

public static class ClipboardExtensions
{
    public static async Task<string?> GetClipboardTextAsync(this IClipboardProvider clipboardProvider)
    {
        if(clipboardProvider.Clipboard.GetClipboard is null) return null;
        return await clipboardProvider.Clipboard.GetClipboard.Invoke();
    }
    
    public static async Task SetClipboardTextAsync(this IClipboardProvider clipboardProvider, string text)
    {
        clipboardProvider.Clipboard.ClipboardContentToSend = text;
    }
}

public static class WindowBaseViewModelClipboardExtensions
{
    public static async Task<string?> GetClipboardTextAsync(this WindowBaseViewModel vm)
    {
        if(vm is not IClipboardProvider clipboardProvider) return null;
        if(clipboardProvider.Clipboard.GetClipboard is null) return null;
        return await clipboardProvider.Clipboard.GetClipboard.Invoke();
    }
    
    public static async Task<bool> SetClipboardTextAsync(this WindowBaseViewModel vm, string text)
    {
        if(vm is not IClipboardProvider clipboardProvider) return false;
        clipboardProvider.Clipboard.ClipboardContentToSend = text;
        return true;
    }
}