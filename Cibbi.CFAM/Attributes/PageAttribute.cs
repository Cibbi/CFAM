namespace Cibbi.CFAM.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PageAttribute : Attribute
{
    public string PageListing { get; }
    public string Path { get; }
    public string Icon { get; }
    public int Priority { get; }

    public PageAttribute(string path, string icon, int priority = 0)
    {
        PageListing = "";
        Path = path;
        Icon = icon;
        Priority = priority;
    }
    public PageAttribute(string listing, string path, string icon, int priority = 0)
    {
        PageListing = listing;
        Path = path;
        Icon = icon;
        Priority = priority;
    }
}
