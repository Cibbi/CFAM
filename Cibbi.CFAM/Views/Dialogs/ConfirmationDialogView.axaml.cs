using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.ViewModels.Dialogs;
using ReactiveUI;

namespace Cibbi.CFAM.Views.Dialogs;

public partial class ConfirmationDialogView : CFAMUserControl<ConfirmationDialogViewModel>
{
    public ConfirmationDialogView()
    {
        InitializeComponent();

        this.WhenActivated(disposable =>
        {
            if(ViewModel is null) return;
            if(ViewModel.CancelIsDefault)
                CancelButton.Classes.Add("accent");
            else
                ConfirmButton.Classes.Add("accent");
        });
    }
}