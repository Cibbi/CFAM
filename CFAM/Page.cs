namespace CFAM;

public class Page
{
    public string Name { get; set; }
    public string IconName { get; set; }
    public Type PageType { get; set; }

    public Page(Type pageType, string name, string iconName)
    {
        Name = name;
        IconName = iconName;
        PageType = pageType;
    }
}