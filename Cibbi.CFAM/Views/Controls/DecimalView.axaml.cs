﻿using Avalonia.ReactiveUI;
using Cibbi.CFAM.ViewModels;

namespace Cibbi.CFAM.Views.Controls;

public partial class DecimalView : ReactiveUserControl<PropertyValueViewModel<decimal>>
{
    public DecimalView()
    {
        InitializeComponent();
    }
}