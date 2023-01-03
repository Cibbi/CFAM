using FluentAvalonia.FluentIcons;
using FluentAvalonia.UI.Controls;

namespace Cibbi.CFAM.Services;

public class IconsProvider : IIconsProvider
{
    private readonly Dictionary<string, Func<IconElement>> _icons = new();

    public void AddIconFactory(string name, Func<IconElement> factory)
    {
        if (!_icons.TryAdd(name, factory))
            _icons[name] = factory;
    }

    public IconsProvider WithIconFactory(string name, Func<IconElement> factory)
    {
        AddIconFactory(name, factory);
        return this;
    }

    public IconElement GetIconFromName(string name)
    {
        return _icons.TryGetValue(name, out var iconFactory) ?
            iconFactory.Invoke() : 
            new FluentIcon{Icon = FluentIconSymbol.ErrorCircle16Filled};
    }
}