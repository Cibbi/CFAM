using Avalonia.Controls;
using CFAM.Attributes.AutoControl;

namespace CFAM.Views.Controls;

[ControlFor(typeof(double))]
public partial class DoubleView : UserControl
{
    public DoubleView()
    {
        InitializeComponent();
    }
}