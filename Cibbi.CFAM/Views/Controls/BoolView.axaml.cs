using Avalonia.ReactiveUI;
using Cibbi.CFAM.ViewModels;

namespace Cibbi.CFAM.Views.Controls;

public partial class BoolView : ReactiveUserControl<PropertyValueViewModel<bool>>
{
    public BoolView()
    {
        InitializeComponent();
    }
}