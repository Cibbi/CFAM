using Avalonia.Controls;
using CFAM.Attributes.AutoControl;

namespace CFAM.Views.Controls;

[ControlFor(typeof(long))]
public partial class LongView : UserControl
{
    public LongView()
    {
        InitializeComponent();
    }
}