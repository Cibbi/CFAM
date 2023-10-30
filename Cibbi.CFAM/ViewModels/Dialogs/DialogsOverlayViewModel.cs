﻿using System.Reactive;
using PropertyChanged.SourceGenerator;
using ReactiveUI;

namespace Cibbi.CFAM.ViewModels.Dialogs;

public partial class DialogsOverlayViewModel : ViewModelBase
{
    [Notify] private DialogViewModel? _currentDialog;
    [Notify] private bool _isDialogOpen;
    
    private readonly object _lockObject = new();
    
    public ReactiveCommand<Unit, Unit> CloseCurrentDialogCommand { get; }

    public DialogsOverlayViewModel()
    {
        CloseCurrentDialogCommand = ReactiveCommand.Create(CloseCurrentDialog);
    }

    private void OnCurrentDialogChanged()
    {
        lock (_lockObject)
        {
            if (CurrentDialog is null)
            {
                IsDialogOpen = false;
                return;
            }

            CurrentDialog.CloseCommand = CloseCurrentDialogCommand;
            IsDialogOpen = true;
        }
    }

    private void CloseCurrentDialog()
    {
        lock (_lockObject)
        {
            IsDialogOpen = false;
            _currentDialog = null;
        }
    }
}