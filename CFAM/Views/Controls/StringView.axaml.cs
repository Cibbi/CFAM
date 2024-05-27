using Avalonia.Controls;
using CFAM.Attributes.AutoControl;

namespace CFAM.Views.Controls;

[ControlFor(typeof(string))]
public partial class StringView : UserControl
{
    public StringView()
    {
        InitializeComponent();
    }
}