using CFAM.ViewModels.Dialogs;
using ReactiveUI;

namespace CFAM.Views.Dialogs;

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