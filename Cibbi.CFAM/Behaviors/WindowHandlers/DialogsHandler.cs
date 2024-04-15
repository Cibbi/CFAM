﻿using Avalonia;
using Cibbi.CFAM.ViewModels;
using Cibbi.CFAM.ViewModels.WindowServices;

namespace Cibbi.CFAM.Behaviors.WindowHandlers;

public class DialogsHandler : WindowServiceHandler
{
    public static readonly StyledProperty<OverlaysHandler> OverlaysHandlerProperty =
        AvaloniaProperty.Register<DialogsHandler, OverlaysHandler>(nameof(OverlaysHandler));

    private Dialogs _dialogs= new ();
    
    public OverlaysHandler OverlaysHandler
    {
        get => GetValue(OverlaysHandlerProperty);
        set => SetValue(OverlaysHandlerProperty, value);
    }

    protected override void OnDataContextChanged()
    {
        base.OnDataContextChanged();
        if (OverlaysHandler is null) return;
        var overlayService = OverlaysHandler.GetOverlaysService();
        if (overlayService.GetOverlay("Dialogs") is not null)
        {
            overlayService.RemoveOverlay("Dialogs");
        }
        overlayService.AddOverlay("Dialogs",
            new OverlayViewModel(_dialogs._dialogsOverlay) {IsEnabled = true, ZIndex = 1000});
        Provider?.AddService(_dialogs);


    }
}