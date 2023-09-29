using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data;
using Cibbi.CFAM.Extensions;
using Cibbi.CFAM.ViewModels;
using FluentAvalonia.UI.Controls;
using ReactiveUI;
using Splat;
using TaskDialogViewModel = Cibbi.CFAM.FluentAvalonia.ViewModels.TaskDialogViewModel;

namespace Cibbi.CFAM.FluentAvalonia.Services;

public enum DialogResult
{
    None,
    Primary,
    Secondary,
}

public class DialogProvider
{
    private readonly IViewLocator _locator = Locator.Current.GetRequiredService<IViewLocator>();
    
    public async Task<string?> ShowOpenFolderDialogAsync(string title)
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
    
    public async Task<string[]?> ShowOpenFileDialogAsync(string title, string? directory = null, string? initialFileName = null, 
        List<FileDialogFilter>? filters = null, bool allowMultiple = false)
    {
        if (Application.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime
            applicationLifetime2)
            throw new NotSupportedException("The file dialog is not supported in non desktop applications");
        
        Window? window1 = null;
        foreach (var window2 in applicationLifetime2.Windows)
        {
            if (!window2.IsActive) continue;
            window1 = window2;
            break;
        }
        window1 ??= applicationLifetime2.MainWindow;
        
        var dialog = new OpenFileDialog
        {
            Title = title,
            Filters = filters,
            Directory = directory,
            InitialFileName = initialFileName,
            AllowMultiple = allowMultiple
        };

        return await dialog.ShowAsync(window1);

    }
    
    public async Task<string?> ShowSaveFileDialogAsync(string title, string? directory = null, string? defaultExtension = null, string? initialFileName = null,
        List<FileDialogFilter>? filters = null)
    {
        if (Application.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime
            applicationLifetime2)
            throw new NotSupportedException("The file dialog is not supported in non desktop applications");
        
        Window? window1 = null;
        foreach (var window2 in applicationLifetime2.Windows)
        {
            if (!window2.IsActive) continue;
            window1 = window2;
            break;
        }
        window1 ??= applicationLifetime2.MainWindow;
        
        var dialog = new SaveFileDialog()
        {
            Title = title,
            Filters = filters,
            Directory = directory,
            DefaultExtension = defaultExtension,
            InitialFileName = initialFileName
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

    public async Task<DialogResult> ShowCustomDialogAsync(ViewModelBase? dialogContent, string title, string closeButtonText = "Cancel",  string primaryButtonText = "", string secondaryButtonText = "")
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

        if (dialogContent is not null)
        {
            var view = _locator.ResolveView(dialogContent);
            if (view is null) throw new ArgumentOutOfRangeException(nameof(ViewModelBase));
            view.ViewModel = dialogContent;
            dialog.Content = view;
        }

        ContentDialogResult result = await dialog.ShowAsync();

        return result switch
        {
            ContentDialogResult.None => DialogResult.None,
            ContentDialogResult.Primary => DialogResult.Primary,
            ContentDialogResult.Secondary => DialogResult.Secondary,
            _ => DialogResult.None
        };
    }
    
    public TaskDialogViewModel GetTaskDialog()
    {
        
        var dialog = new TaskDialog()
        {
            [!TaskDialog.HeaderProperty] = new Binding("Header", BindingMode.OneWay),
            [!TaskDialog.SubHeaderProperty] = new Binding("SubHeader", BindingMode.OneWay),
            [!TaskDialog.ContentProperty] = new Binding("Content", BindingMode.OneWay),

        };
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime applicationLifetime2)
        {
            Window window1 = (Window) null;
            foreach (Window window2 in (IEnumerable<Window>) applicationLifetime2.Windows)
            {
                if (window2.IsActive)
                {
                    window1 = window2;
                    break;
                }
            }
            if (window1 == null)
                window1 = applicationLifetime2.MainWindow;
            dialog.XamlRoot = window1;
        }
        else if (Application.Current.ApplicationLifetime is ISingleViewApplicationLifetime applicationLifetime1)
            dialog.XamlRoot = applicationLifetime1.MainView;
        
        TaskDialogViewModel viewModel = new TaskDialogViewModel(dialog.ShowAsync(true), 
            x => dialog.SetProgressBarState(x, TaskDialogProgressState.Normal), 
            () => dialog.Hide());
        dialog.ShowProgressBar = true;
        dialog.DataContext = viewModel;

        return viewModel;
    }
}