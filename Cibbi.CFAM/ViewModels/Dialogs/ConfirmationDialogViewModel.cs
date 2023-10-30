using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Cibbi.CFAM.ViewModels.Dialogs;

public class ConfirmationDialogViewModel : DialogViewModel
{
    public string Title { get; init; } 
    public string Message { get; init; }
    
    public string ConfirmText { get; set; } = "Conferma";
    public string CancelText { get; set; } = "Annulla";
    
    public bool CancelIsDefault { get; set; }
    
    public CombinedReactiveCommand<Unit, Unit> ConfirmCommand { get; }
    public CombinedReactiveCommand<Unit, Unit> CancelCommand { get; }
    
    public ConfirmationDialogViewModel(string title, string message, ReactiveCommand<Unit, Unit> confirmCommand, ReactiveCommand<Unit, Unit>? cancelCommand = default)
    {
        Title = title;
        Message = message;
        var closeCommand =  ReactiveCommand.CreateFromTask(CloseDialog);
        ConfirmCommand = ReactiveCommand.CreateCombined(new[]{ confirmCommand, closeCommand });
        CancelCommand = ReactiveCommand.CreateCombined(cancelCommand is not null ? new[]{ cancelCommand, closeCommand } : new[]{ closeCommand });
    }
    
    private async Task CloseDialog()
    {
        await (CloseCommand?.Execute() ?? Observable.Return(Unit.Default));
    }
}