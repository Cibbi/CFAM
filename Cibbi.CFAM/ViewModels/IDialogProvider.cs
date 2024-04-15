using System.Reactive;
using Cibbi.CFAM.ViewModels.Dialogs;
using ReactiveUI;

namespace Cibbi.CFAM.ViewModels;

public interface IDialogProvider
{
    public DialogsOverlayViewModel DialogsOverlay { get; }
}

