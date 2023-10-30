using System.Reactive;
using ReactiveUI;

namespace Cibbi.CFAM.ViewModels.Dialogs;

public abstract class DialogViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, Unit>? CloseCommand { get; set; }
}