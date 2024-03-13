using Avalonia.Controls;

namespace Cibbi.CFAM.Services;

public interface IViewLocator
{
    Control? FindView<T>(T? viewModel);
}