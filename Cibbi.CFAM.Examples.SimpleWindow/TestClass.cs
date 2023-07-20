using PropertyChanged.SourceGenerator;

namespace Cibbi.CFAM.Examples.SimpleWindow;

public partial class TestClass
{
    [Notify] private string _testString = "";
    [Notify] private string _testString2 = "";
    [Notify] [PropertyAttribute("Cibbi.CFAM.Attributes.IgnoreProperty")] private string _ignoredString = "";
    [Notify] private int _testInt;
    private int NonVisibleInt { get; set; }
    [Notify] private float _testFloat;
    [Notify] private TestClass2 _testChild = new();

}

public partial class TestClass2 : TestBaseClass
{
    [Notify] private string _testString = "";
    [Notify] private bool _testBool;
    private int NonVisibleInt { get; set; }
    [Notify] private float _testFloat;
}

public partial class TestBaseClass
{
    [Notify] private string _ignoredString = "";
    [Notify] [PropertyAttribute("Cibbi.CFAM.Attributes.IncludeProperty")] private string _visibleString = "";
}