using Avalonia.Controls;
using CFAM.Attributes.AutoControl;

namespace CFAM.Views.Controls;

[ControlFor(typeof(int))]
public partial class IntView : UserControl
{
    public IntView()
    {
        InitializeComponent();
    }
}