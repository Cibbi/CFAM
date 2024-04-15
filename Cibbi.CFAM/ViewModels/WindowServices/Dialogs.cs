using System.Reactive;
using Cibbi.CFAM.ViewModels.Dialogs;
using ReactiveUI;

namespace Cibbi.CFAM.ViewModels.WindowServices;

public class Dialogs : IWindowService
{
    internal readonly DialogsOverlayViewModel _dialogsOverlay = new ();
    
    public void ShowDialog(DialogViewModel dialog)
    {
        _dialogsOverlay.CurrentDialog = dialog;
    }
    
    public void CloseDialog()
    {
        _dialogsOverlay.CurrentDialog = null;
    }
    
    public void ShowConfirmationDialog(string title, string message, 
        ReactiveCommand<Unit, Unit> onConfirm, ReactiveCommand<Unit,Unit>? onCancel = default, bool cancelIsDefault = false)
    {
        ShowDialog(new ConfirmationDialogViewModel(title, message, onConfirm, onCancel)
        {
            CancelIsDefault = cancelIsDefault
        });
    }
    
    public void ShowConfirmationDialog(string title, string message, string conFirmText,
        ReactiveCommand<Unit, Unit> onConfirm, ReactiveCommand<Unit,Unit>? onCancel = default, bool cancelIsDefault = false)
    {
        ShowDialog(new ConfirmationDialogViewModel(title, message, onConfirm, onCancel)
        {
            ConfirmText = conFirmText, 
            CancelIsDefault = cancelIsDefault
        });
    }
    
    public void ShowConfirmationDialog(string title, string message, string conFirmText, string cancelText, 
        ReactiveCommand<Unit, Unit> onConfirm, ReactiveCommand<Unit,Unit>? onCancel = default, bool cancelIsDefault = false)
    {
        ShowDialog(new ConfirmationDialogViewModel(title, message, onConfirm, onCancel)
        {
            ConfirmText = conFirmText, 
            CancelText = cancelText, 
            CancelIsDefault = cancelIsDefault
        });
    }
}