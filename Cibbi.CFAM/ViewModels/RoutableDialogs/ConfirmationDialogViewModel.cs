using System.Reactive;
using ReactiveUI;

namespace Cibbi.CFAM.ViewModels.RoutableDialogs;

public class ConfirmationDialogViewModel : RoutableViewModel
{
    public override string UrlPathSegment => "confirmation";
    
    public required string Title { get; init; } 
    public required string Message { get; init; }
    
    public string ConfirmText { get; set; } = "Conferma";
    public string CancelText { get; set; } = "Annulla";
    
    public ReactiveCommand<Unit, Unit> ConfirmCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }
    
    public ConfirmationDialogViewModel(IScreen screen, ReactiveCommand<Unit, Unit> confirmCommand) : base(screen)
    {
        ConfirmCommand = confirmCommand;
        CancelCommand = ReactiveCommand.Create(DefaultCancel);
    }
    
    public ConfirmationDialogViewModel(IScreen screen, ReactiveCommand<Unit, Unit> confirmCommand, ReactiveCommand<Unit, Unit> cancelCommand) : base(screen)
    {
        ConfirmCommand = confirmCommand;
        CancelCommand = cancelCommand;
    }
    
    private void DefaultCancel()
    {
        HostScreen.Router.NavigateBack.Execute();
    }
}