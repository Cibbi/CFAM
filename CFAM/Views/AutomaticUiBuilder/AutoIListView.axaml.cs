using System.Collections;
using Avalonia.Controls;
using CFAM.Extensions;
using CFAM.Services;
using CFAM.ViewModels;
using PropertyChanged.SourceGenerator;
using Splat;

namespace CFAM.Views.AutomaticUiBuilder;

public partial class AutoIListView : UserControl
{
    
    private readonly IViewLocator _viewLocator = Locator.Current.GetRequiredService<IViewLocator>();
    
    public AutoIListView()
    {
        InitializeComponent();
    }

    private List<ListItem> _items;

    private IList _context;

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is not IList list)
        {
            throw new Exception($"DataContext of Type {DataContext?.GetType()} is not an IEnumerable");
        }

        _context = list;
        _items = [];

        ListBox listBox = new();
        int i = 0;
        foreach (var element in list)
        {
            var i1 = i;
            var item = new ListItem(element, () =>
            {
                _context[i1] = _items[i1].Item;
            });
            
            var view = _viewLocator.FindView(item);
            view.DataContext = item;
            listBox.Items.Add(view);
            _items.Add(item);
            i++;
        }
        Content = listBox;
        
    }
}

internal partial class ListItem : ViewModelBase
{
    private Action? _onChangedCallBack;
    public ListItem(object item, Action onChangedCallback)
    {
        Item = item;
        _onChangedCallBack = onChangedCallback;
    }

    [Notify] 
    [PropertyAttribute("[CFAM.Attributes.AutoControl.VisualName(\"\")]")]
    public object _item;

    public void OnItemChanged()
    {
        _onChangedCallBack?.Invoke();
    }
}