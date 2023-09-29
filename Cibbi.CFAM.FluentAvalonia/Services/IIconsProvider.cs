using FluentAvalonia.UI.Controls;

namespace Cibbi.CFAM.FluentAvalonia.Services;

public interface IIconsProvider
{
    IconSource GetIconFromName(string name);
}