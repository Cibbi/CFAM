using Avalonia.Controls;
using Cibbi.CFAM.Views;
using ReactiveUI;

namespace Cibbi.CFAM;

public class BaseViewLocator : IViewLocator
{
    private static readonly Dictionary<Type, Type?> _viewModelToViewMapping = new();
        
    public virtual IViewFor ResolveView<T>(T viewModel, string? contract = null)
    {
        Type? vmType = viewModel?.GetType();
        if (vmType is null) throw new ArgumentNullException(nameof(viewModel));
        if(_viewModelToViewMapping.TryGetValue(vmType, out var vType)) return (IViewFor)Activator.CreateInstance(vType!)!;

        string? name = vmType.AssemblyQualifiedName!.Replace("ViewModel", "View");
        if(name is null) throw new ArgumentOutOfRangeException(nameof(viewModel));
        var type = Type.GetType(name);
        if (type is null || !type.IsAssignableTo(typeof(ContentControl)))
        {
            _viewModelToViewMapping.Add(vmType, typeof(AutoView));
            return (IViewFor)Activator.CreateInstance(typeof(AutoView))!;
        }
        _viewModelToViewMapping.Add(vmType, type);
        return (IViewFor)Activator.CreateInstance(type)!;
    }
}