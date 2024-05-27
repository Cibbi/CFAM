using Avalonia.Controls;
using CFAM.Attributes.AutoControl;

namespace CFAM.Views.Controls;

[ControlFor(typeof(bool))]
public partial class BoolView : UserControl
{
    public BoolView()
    {
        InitializeComponent();
    }
}