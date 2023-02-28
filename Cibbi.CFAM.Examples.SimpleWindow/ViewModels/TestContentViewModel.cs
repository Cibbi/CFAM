using System;
using System.Reactive.Linq;
using Cibbi.CFAM.ViewModels;
using PropertyChanged.SourceGenerator;
using ReactiveUI;

namespace Cibbi.CFAM.Examples.SimpleWindow.ViewModels;

public partial class TestContentViewModel : ViewModelBase
{
    [Notify] private TestClass _testObject = new();

    private int count = 0;
    public TestContentViewModel()
    {
        Observable.Interval(TimeSpan.FromSeconds(1)).ObserveOn(RxApp.TaskpoolScheduler).Subscribe(_ =>
        {
            TestObject.TestString = ""+count;
            count++;
        });
    }
}