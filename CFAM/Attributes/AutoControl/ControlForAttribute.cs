namespace CFAM.Attributes.AutoControl;

[AttributeUsage(AttributeTargets.Class)]
public class ControlForAttribute : Attribute
{
    public Type PropertyType { get; set; }

    public ControlForAttribute(Type propertyType)
    {
        PropertyType = propertyType;
    }
}