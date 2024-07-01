using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Launcher.Views.Components;

public partial class PageHeader : UserControl {
    public PageHeader() {
        InitializeComponent();
    }

    private void ShutDownApp(object sender, RoutedEventArgs e) {
        Application.Current.Shutdown();
    }

    private void ColapsApp(object sender, RoutedEventArgs e) {
        Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }

    private void WindowMove(object sender, MouseButtonEventArgs e) {
        if (e.ChangedButton == MouseButton.Left) {
            Window.GetWindow(this).DragMove();
        }
    }
}
