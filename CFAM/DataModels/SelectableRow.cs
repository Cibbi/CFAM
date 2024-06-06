using CFAM.ViewModels;
using PropertyChanged.SourceGenerator;

namespace CFAM.DataModels;

public class SelectableRow<T> : ViewModelBase
{
    [Notify] private T _item;
    [Notify] private bool _isSelected;

    public SelectableRow(T item)
    {
        _item = item;
    }
}