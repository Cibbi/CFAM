using System.Reactive;
using Cibbi.CFAM.ViewModels.Dialogs;
using ReactiveUI;

namespace Cibbi.CFAM.ViewModels;

public interface IDialogProvider
{
    public DialogsOverlayViewModel DialogsOverlay { get; }
}

public static class DialogProviderExtensions
{
    public static void ShowDialog(this IDialogProvider dialogProvider, DialogViewModel dialog)
    {
        dialogProvider.DialogsOverlay.CurrentDialog = dialog;
    }
    
    public static void CloseDialog(this IDialogProvider dialogProvider)
    {
        dialogProvider.DialogsOverlay.CurrentDialog = null;
    }
    
    public static void ShowConfirmationDialog(this IDialogProvider dialogProvider, string title, string message, 
        ReactiveCommand<Unit, Unit> onConfirm, ReactiveCommand<Unit,Unit>? onCancel = default, bool cancelIsDefault = false)
    {
        dialogProvider.ShowDialog(new ConfirmationDialogViewModel(title, message, onConfirm, onCancel)
        {
            CancelIsDefault = cancelIsDefault
        });
    }
    
    public static void ShowConfirmationDialog(this IDialogProvider dialogProvider, string title, string message, string conFirmText,
        ReactiveCommand<Unit, Unit> onConfirm, ReactiveCommand<Unit,Unit>? onCancel = default, bool cancelIsDefault = false)
    {
        dialogProvider.ShowDialog(new ConfirmationDialogViewModel(title, message, onConfirm, onCancel)
        {
            ConfirmText = conFirmText, 
            CancelIsDefault = cancelIsDefault
        });
    }
    
    public static void ShowConfirmationDialog(this IDialogProvider dialogProvider, string title, string message, string conFirmText, string cancelText, 
        ReactiveCommand<Unit, Unit> onConfirm, ReactiveCommand<Unit,Unit>? onCancel = default, bool cancelIsDefault = false)
    {
        dialogProvider.ShowDialog(new ConfirmationDialogViewModel(title, message, onConfirm, onCancel)
        {
            ConfirmText = conFirmText, 
            CancelText = cancelText, 
            CancelIsDefault = cancelIsDefault
        });
    }
}

public static class WindowBaseViewModelDialogProviderExtensions
{
    public static bool ShowDialog(this WindowBaseViewModel vm, DialogViewModel dialog)
    {
        if (vm is not IDialogProvider dialogProvider) return false;
        dialogProvider.DialogsOverlay.CurrentDialog = dialog;
        return true;
    }

    public static bool CloseDialog(this WindowBaseViewModel vm)
    {
        if (vm is not IDialogProvider dialogProvider) return false;
        dialogProvider.DialogsOverlay.CurrentDialog = null;
        return true;
    }

    public static bool ShowConfirmationDialog(this WindowBaseViewModel vm, string title, string message,
        ReactiveCommand<Unit, Unit> onConfirm, ReactiveCommand<Unit, Unit>? onCancel = default,
        bool cancelIsDefault = false)
    {
        if (vm is not IDialogProvider dialogProvider) return false;
        dialogProvider.ShowDialog(new ConfirmationDialogViewModel(title, message, onConfirm, onCancel)
        {
            CancelIsDefault = cancelIsDefault
        });
        return true;
    }

    public static bool ShowConfirmationDialog(this WindowBaseViewModel vm, string title, string message, string confirmText,
        ReactiveCommand<Unit, Unit> onConfirm, ReactiveCommand<Unit,Unit>? onCancel = default, bool cancelIsDefault = false)
    {
        if (vm is not IDialogProvider dialogProvider) return false;
        dialogProvider.ShowDialog(new ConfirmationDialogViewModel(title, message, onConfirm, onCancel)
        {
            ConfirmText = confirmText, 
            CancelIsDefault = cancelIsDefault
        });
        return true;
    }

    public static bool ShowConfirmationDialog(this WindowBaseViewModel vm, string title, string message, string confirmText, string cancelText, 
        ReactiveCommand<Unit, Unit> onConfirm, ReactiveCommand<Unit,Unit>? onCancel = default, bool cancelIsDefault = false)
    {
        if (vm is not IDialogProvider dialogProvider) return false;
        dialogProvider.ShowDialog(new ConfirmationDialogViewModel(title, message, onConfirm, onCancel)
        {
            ConfirmText = confirmText, 
            CancelText = cancelText, 
            CancelIsDefault = cancelIsDefault
        });
        return true;
    }
}