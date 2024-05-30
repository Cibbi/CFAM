using CFAM.Attributes.AutoControl;
using CFAM.ViewModels;

namespace CFAM.Examples;

public class ExampleAutoObject : ViewModelBase
{
    public int PublicInteger { get; set; }
    public string PublicString { get; set; }
    [VisualName("Custom Named String")]
    public string PublicString2 { get; set; }
    private int PrivateHiddenInteger { get; set; }
    [IncludeProperty]
    private int PrivateVisibleInteger { get; set; }
    [IgnoreProperty]
    public int PublicHiddenInteger { get; set; }
}