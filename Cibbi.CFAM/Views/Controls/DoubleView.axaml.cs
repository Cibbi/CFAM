using Avalonia.ReactiveUI;
using Cibbi.CFAM.ViewModels;

namespace Cibbi.CFAM.Views.Controls;

public partial class DoubleView : ReactiveUserControl<PropertyValueViewModel<double>>
{
    public DoubleView()
    {
        InitializeComponent();
    }
}