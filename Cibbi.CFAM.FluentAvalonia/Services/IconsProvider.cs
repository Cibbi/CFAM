using Cibbi.CFAM.Services;
using FluentAvalonia.UI.Controls;

namespace Cibbi.CFAM.FluentAvalonia.Services;

public class IconsProvider : IIconsProvider
{
    private readonly Dictionary<string, Func<IconSource>> _icons = new();

    public void AddIconFactory(string name, Func<IconSource> factory)
    {
        if (!_icons.TryAdd(name, factory))
            _icons[name] = factory;
    }

    public IconsProvider WithIconFactory(string name, Func<IconSource> factory)
    {
        AddIconFactory(name, factory);
        return this;
    }

    public IconSource GetIconFromName(string name)
    {
        return _icons.TryGetValue(name, out var iconFactory)
            ? iconFactory.Invoke()
            : new SymbolIconSource(){ Symbol = Symbol.Stop};
    }
}