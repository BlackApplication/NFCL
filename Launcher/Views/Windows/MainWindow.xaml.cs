using Core.ViewModels;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

namespace Launcher.Views.Windows;

[MvxWindowPresentation]
[MvxViewFor(typeof(MainViewModel))]
public partial class MainWindow : MvxWindow {
    public MainWindow() {
        InitializeComponent();
    }
}
