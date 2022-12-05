using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Cibbi.CFAM.ViewModels;
using FluentAvalonia.UI.Controls;
using ReactiveUI;

namespace Cibbi.CFAM.Services;

public enum DialogResult
{
    None,
    Primary,
    Secondary,
}

public class DialogProvider
{
    private readonly IViewLocator _locator = AvaloniaLocator.Current.GetRequiredService<IViewLocator>();
    
    public async Task<string?> ShowFolderDialogAsync(string title)
    {
        if (Application.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime
            applicationLifetime2)
            throw new NotSupportedException("The folder dialog is not supported in non desktop applications");
        
        Window? window1 = null;
        foreach (var window2 in applicationLifetime2.Windows)
        {
            if (!window2.IsActive) continue;
            window1 = window2;
            break;
        }
        window1 ??= applicationLifetime2.MainWindow;
        
        var dialog = new OpenFolderDialog
        {
            Title = title
        };
        
        return await dialog.ShowAsync(window1);

    }
    
    public async Task<string?> ShowFolderDialogAsync(string title, Window window)
    {
        var dialog = new OpenFolderDialog
        {
            Title = title
        };

        return await dialog.ShowAsync(window);
    }
    
    public async Task<DialogResult> ShowCustomDialogAsync(ViewModelBase dialogContent, string title, string closeButtonText = "Cancel",  string primaryButtonText = "", string secondaryButtonText = "")
    {
        var dialog = new ContentDialog
        {
            Title = title,
            CloseButtonText = closeButtonText
        };

        if (string.IsNullOrWhiteSpace(primaryButtonText))
            dialog.IsPrimaryButtonEnabled = false;
        else
            dialog.PrimaryButtonText = primaryButtonText;
        
        if (string.IsNullOrWhiteSpace(secondaryButtonText))
            dialog.IsSecondaryButtonEnabled = false;
        else
            dialog.SecondaryButtonText = secondaryButtonText;

        var view = _locator.ResolveView(dialogContent);
        if(view is null) throw new ArgumentOutOfRangeException(nameof(ViewModelBase));
        view.ViewModel = dialogContent;
        dialog.Content = view;

        ContentDialogResult result = await dialog.ShowAsync();

        return result switch
        {
            ContentDialogResult.None => DialogResult.None,
            ContentDialogResult.Primary => DialogResult.Primary,
            ContentDialogResult.Secondary => DialogResult.Secondary,
            _ => DialogResult.None
        };
    }
}