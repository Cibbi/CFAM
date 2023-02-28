using Cibbi.CFAM.Views.Controls;

namespace Cibbi.CFAM;

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
}