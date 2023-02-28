using Avalonia.ReactiveUI;
using Cibbi.CFAM.ViewModels;

namespace Cibbi.CFAM.Views.Controls;

public partial class StringView : ReactiveUserControl<PropertyValueViewModel<string>>
{
    public StringView()
    {
        InitializeComponent();
    }
}