using Avalonia.Controls;
using Avalonia.Input.Platform;
using Cibbi.CFAM.ViewModels;
using Cibbi.CFAM.ViewModels.WindowServices;

namespace Cibbi.CFAM.Behaviors.WindowHandlers;

public class ClipboardHandler : WindowServiceHandler
{
    private Clipboard _clipboard;
    private IClipboard? _avaloniaClipboard;

    public ClipboardHandler()
    {
        _clipboard = new Clipboard();
        _clipboard.GetClipboard = GetClipboardTextAsync;
        _clipboard.SetClipboard = SetClipboardTextAsync;
    }
    protected override void OnDataContextChanged()
    {
        _avaloniaClipboard = TopLevel.GetTopLevel(AssociatedObject)?.Clipboard;
        Provider?.AddService(_clipboard);
    }

    protected override void OnDetachedFromVisualTree()
    {
        base.OnDetachedFromVisualTree();
        _avaloniaClipboard = null;
    }

    private Task<string?> GetClipboardTextAsync()
    {
        if (_avaloniaClipboard is null)
        {
            throw new Exception("Clipboard is not available");
        }

        return _avaloniaClipboard.GetTextAsync();
    }

    private Task SetClipboardTextAsync(string text)
    {
        if (_avaloniaClipboard is null)
        {
            throw new Exception("Clipboard is not available");
        }

        return _avaloniaClipboard.SetTextAsync(text);
    }
}