using PropertyChanged.SourceGenerator;

namespace CFAM.ViewModels;

public abstract partial class WindowBaseViewModel : ViewModelBase
{
    [Notify] private string _windowName = "";
    [Notify] private float _windowWidth = float.NaN;// 1280;
    [Notify] private float _windowHeight = float.NaN;// 750;
    [Notify] private WindowState _windowState = WindowState.Normal;
    [Notify] private bool _hasCustomTitleBar;
   
    
    public WindowBaseViewModel (string title)
    {
        WindowName = title;
    }
    
    public WindowBaseViewModel()
    { }
}

public enum WindowState
{
    Normal,
    Minimized,
    Maximized,
    //FullScreen
}