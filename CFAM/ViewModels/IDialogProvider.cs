using CFAM.ViewModels.Dialogs;

namespace CFAM.ViewModels;

public interface IDialogProvider
{
    public DialogsOverlayViewModel DialogsOverlay { get; }
}

