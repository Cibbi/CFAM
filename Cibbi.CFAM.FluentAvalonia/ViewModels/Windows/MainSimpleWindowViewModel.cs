using Cibbi.CFAM.ViewModels;
using PropertyChanged.SourceGenerator;

namespace Cibbi.CFAM.FluentAvalonia.ViewModels.Windows;

public partial class MainSimpleWindowViewModel : WindowBaseViewModel
{
    [Notify] private ViewModelBase _content = null!;
}