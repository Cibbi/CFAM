using Avalonia.Controls;
using CFAM.Services;
using CFAM.Views.AutomaticUiBuilder;


namespace CFAM;

public class BaseViewLocator : IViewLocator
{
    private static readonly Dictionary<Type, Type?> _viewModelToViewMapping = new();
        
    public virtual Control FindView<T>(T? viewModel)
    {
        var vmType = viewModel?.GetType();
        if (vmType is null) throw new ArgumentNullException(nameof(viewModel));
        if(_viewModelToViewMapping.TryGetValue(vmType, out var vType)) return (Control)Activator.CreateInstance(vType!)!;

        string? name = vmType.AssemblyQualifiedName!.Replace("ViewModel", "View");
        if(name is null) throw new ArgumentOutOfRangeException(nameof(viewModel));
        var type = Type.GetType(name);
        if (type is null || !type.IsAssignableTo(typeof(Control)))
        {
            _viewModelToViewMapping.Add(vmType, typeof(AutoView));
            return (Control)Activator.CreateInstance(typeof(AutoView))!;
        }
        _viewModelToViewMapping.Add(vmType, type);
        return (Control)Activator.CreateInstance(type)!;
    }
}