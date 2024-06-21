using Core.ViewModels;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Launcher.Views.Pages;

[MvxContentPresentation]
[MvxViewFor(typeof(WelcomeViewModel))]
public partial class WelcomeView : MvxWpfView {
    public WelcomeView() {
        InitializeComponent();
    }

    private void ShutDownApp(object sender, RoutedEventArgs e) {
        Application.Current.Shutdown();
    }

    private void WindowMove(object sender, MouseButtonEventArgs e) {
        if (e.ChangedButton == MouseButton.Left) {
            Window.GetWindow(this).DragMove();
        }
    }
}
