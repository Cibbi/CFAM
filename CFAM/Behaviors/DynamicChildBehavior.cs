using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using CFAM.ViewModels;

namespace CFAM.Behaviors
{
    public class DynamicChildBehavior : Trigger<Panel>
    {
        public static readonly StyledProperty<ViewModelBase?> ContentProperty =
            AvaloniaProperty.Register<DynamicChildBehavior, ViewModelBase?>(nameof(Content));

        public object? Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        
        private CompositeDisposable? _disposable;

        protected override void OnAttachedToVisualTree()
        {
            base.OnAttachedToVisualTree();

            _disposable = new CompositeDisposable
            {
                this.GetObservable(ContentProperty).Subscribe(newValue =>
                {
                    UpdateChild(newValue);
                })
            };

            // Initial setup
            UpdateChild(Content);
        }

        protected override void OnDetachedFromVisualTree()
        {
            base.OnDetachedFromVisualTree();

            _disposable?.Dispose();
            _disposable = null;
            AssociatedObject?.Children.Clear();
        }

        private void UpdateChild(object? newValue)
        {
            if (AssociatedObject == null || newValue == null || CFAMSettings.ViewLocator.FindView(newValue) is not Control control)
            {
                AssociatedObject?.Children.Clear();
            }
            else
            {
                control.DataContext = newValue;
                AssociatedObject.Children.Clear();
                AssociatedObject.Children.Add(control);
            }
        }
    }
}