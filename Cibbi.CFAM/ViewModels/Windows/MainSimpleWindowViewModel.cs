using PropertyChanged.SourceGenerator;

namespace Cibbi.CFAM.ViewModels.Windows;

public partial class MainSimpleWindowViewModel : WindowBaseViewModel
{
    [Notify] private ViewModelBase _content = null!;
}