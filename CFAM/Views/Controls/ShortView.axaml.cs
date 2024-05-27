using Avalonia.Controls;
using CFAM.Attributes.AutoControl;

namespace CFAM.Views.Controls;

[ControlFor(typeof(short))]
public partial class ShortView : UserControl
{
    public ShortView()
    {
        InitializeComponent();
    }
}