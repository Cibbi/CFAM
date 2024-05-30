namespace CFAM.Attributes.AutoControl;

[AttributeUsage(AttributeTargets.Property)]
public class VisualNameAttribute : Attribute
{
    public string Name { get; }
    public VisualNameAttribute(string name)
    {
        Name = name;
    }
}