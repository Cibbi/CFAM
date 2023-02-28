using Avalonia.ReactiveUI;
using Cibbi.CFAM.ViewModels;

namespace Cibbi.CFAM.Views.Controls;

public partial class LongView : ReactiveUserControl<PropertyValueViewModel<long>>
{
    public LongView()
    {
        InitializeComponent();
    }
}