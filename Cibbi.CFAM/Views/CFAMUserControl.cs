﻿using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.ReactiveUI;
using Cibbi.CFAM.ViewModels;
using ReactiveUI;

namespace Cibbi.CFAM.Views;

public class CFAMUserControl<T> : ReactiveUserControl<T> where T : ViewModelBase 
{
    public CFAMUserControl()
    {
        this.WhenActivated(disposable =>
        {
            if(ViewModel is null) return;
            if(ViewModel.GetRootViewModel is not null) return;

            ViewModel.GetRootViewModel += GetRoot;
        });
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        if(ViewModel is not null)
            ViewModel.GetRootViewModel -= GetRoot;
        base.OnUnloaded(e);
    }

    private WindowBaseViewModel? GetRoot()
    {
        return TopLevel.GetTopLevel(this)?
            .GetLogicalChildren()
            .OfType<StyledElement>()
            .FirstOrDefault(x => x.DataContext is WindowBaseViewModel)
            ?.DataContext as WindowBaseViewModel;
    }
}