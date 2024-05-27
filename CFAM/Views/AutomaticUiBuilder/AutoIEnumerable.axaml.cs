using System.Collections;
using Avalonia.Controls;
using CFAM.Extensions;
using CFAM.Services;
using Splat;

namespace CFAM.Views.AutomaticUiBuilder;

public partial class AutoIEnumerable : UserControl
{
    
    private readonly IViewLocator _viewLocator = Locator.Current.GetRequiredService<IViewLocator>();
    
    public AutoIEnumerable()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is not IEnumerable enumerable)
        {
            throw new Exception($"DataContext of Type {DataContext?.GetType()} is not an IEnumerable");
        }

        ListBox listBox = new ListBox();
        foreach (var element in enumerable)
        {
            var view = _viewLocator.FindView(element);
            listBox.Items.Add(view);
        }
        
        Content = listBox;
    }
}