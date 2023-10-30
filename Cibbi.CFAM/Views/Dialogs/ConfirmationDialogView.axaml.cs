using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.ViewModels.Dialogs;

namespace Cibbi.CFAM.Views.Dialogs;

public partial class ConfirmationDialogView : CFAMUserControl<ConfirmationDialogViewModel>
{
    public ConfirmationDialogView()
    {
        InitializeComponent();
    }
}