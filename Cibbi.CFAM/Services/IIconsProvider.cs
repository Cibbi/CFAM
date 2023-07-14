using Avalonia.Controls;
using FluentAvalonia.UI.Controls;

namespace Cibbi.CFAM.Services;

public interface IIconsProvider
{
    IconSource GetIconFromName(string name);
}