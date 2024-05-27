using Avalonia.Controls;

namespace CFAM.Services;

public interface IViewLocator
{
    Control? FindView<T>(T? viewModel);
}