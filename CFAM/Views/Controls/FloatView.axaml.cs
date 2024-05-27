using Avalonia.Controls;
using CFAM.Attributes.AutoControl;

namespace CFAM.Views.Controls;

[ControlFor(typeof(float))]
public partial class FloatView : UserControl
{
    public FloatView()
    {
        InitializeComponent();
    }
}