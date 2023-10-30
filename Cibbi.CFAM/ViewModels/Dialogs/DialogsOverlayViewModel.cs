﻿using System.Reactive;
using PropertyChanged.SourceGenerator;
using ReactiveUI;

namespace Cibbi.CFAM.ViewModels.Dialogs;

public partial class DialogsOverlayViewModel : ViewModelBase
{
    [Notify] private DialogViewModel? _currentDialog;
    
    public ReactiveCommand<Unit, Unit> CloseCurrentDialogCommand { get; }

    public DialogsOverlayViewModel()
    {
        CloseCurrentDialogCommand = ReactiveCommand.Create(CloseCurrentDialog);
    }

    private void CloseCurrentDialog()
    {
        _currentDialog = null;
    }
}