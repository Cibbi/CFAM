using Avalonia.ReactiveUI;
using Cibbi.CFAM.ViewModels;

namespace Cibbi.CFAM.Views.Controls;

public partial class ShortView : ReactiveUserControl<PropertyValueViewModel<short>>
{
    public ShortView()
    {
        InitializeComponent();
    }
}