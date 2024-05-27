using Avalonia.Controls;
using CFAM.Attributes.AutoControl;

namespace CFAM.Views.Controls;

[ControlFor(typeof(decimal))]
public partial class DecimalView : UserControl
{
    public DecimalView()
    {
        InitializeComponent();
    }
}