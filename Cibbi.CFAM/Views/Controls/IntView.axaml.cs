using Avalonia.ReactiveUI;
using Cibbi.CFAM.ViewModels;

namespace Cibbi.CFAM.Views.Controls;

public partial class IntView : ReactiveUserControl<PropertyValueViewModel<int>>
{
    public IntView()
    {
        InitializeComponent();
    }
}