using CFAM.Views.Controls;

namespace CFAM;

public static class DefaultViewModelMappings
{
    public static Dictionary<Type, Type?> Instance { get; } = new()
    {
        { typeof(string), typeof(StringView) },
        { typeof(short), typeof(ShortView) },
        { typeof(int), typeof(IntView) },
        { typeof(long), typeof(LongView) },
        { typeof(float), typeof(FloatView) },
        { typeof(double), typeof(DoubleView) },
        { typeof(decimal), typeof(DecimalView) },
        { typeof(bool), typeof(BoolView) }
    };

    public static void AddMapping(Type valueType, Type ControlType)
    {
        Instance.Add(valueType, ControlType);
    }
}