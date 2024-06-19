using MvvmCross.Platforms.Wpf.Views;
using System.Windows.Input;

namespace Launcher.Views.Windows;

public partial class MainWindow : MvxWindow {
    public MainWindow() {
        InitializeComponent();
    }

    private void WindowMove(object sender, MouseButtonEventArgs e) {
        if (e.ChangedButton == MouseButton.Left) {
            this.DragMove();
        }
    }
}
