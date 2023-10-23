using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.ViewModels.RoutableDialogs;

namespace Cibbi.CFAM.Views.RoutableDialogs;

public partial class ConfirmationDialogView : CFAMUserControl<ConfirmationDialogViewModel>
{
    public ConfirmationDialogView()
    {
        InitializeComponent();
    }
}