using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Cibbi.CFAM.ViewModels;

namespace Cibbi.CFAM.Views;

public partial class OverlayView : CFAMUserControl<OverlayViewModel>
{
    public OverlayView()
    {
        InitializeComponent();
    }
}