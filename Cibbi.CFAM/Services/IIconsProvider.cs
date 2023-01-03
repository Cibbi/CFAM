using FluentAvalonia.UI.Controls;

namespace Cibbi.CFAM.Services;

public interface IIconsProvider
{
    IconElement GetIconFromName(string name);
}