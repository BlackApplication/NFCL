using MvvmCross.Platforms.Wpf.Views;
using System.Windows;

namespace Launcher.Views.Pages;

public partial class WelcomeView : MvxWpfView {
    public WelcomeView() {
        InitializeComponent();
    }

    private void ShutDownApp(object sender, RoutedEventArgs e) {
        Application.Current.Shutdown();
    }
}
