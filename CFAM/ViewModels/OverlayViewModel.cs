using PropertyChanged.SourceGenerator;

namespace CFAM.ViewModels;

public partial class OverlayViewModel : ViewModelBase
{
    public ViewModelBase ViewModel { get; init; }
    
    [Notify] private bool _isEnabled;
    
    public int ZIndex { get; set; }
    
    public OverlayViewModel(ViewModelBase viewModel)
    {
        ViewModel = viewModel;
    }
}