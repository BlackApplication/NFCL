using Core.ViewModels;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using System.Windows;

namespace Launcher.Views.Windows;

[MvxWindowPresentation]
[MvxViewFor(typeof(LoginViewModel))]
public partial class LoginWindow : MvxWindow {
    public LoginWindow() {
        InitializeComponent();

        SetWindowPositionFromCenter(250, 150);
    }

    private void SetWindowPositionFromCenter(int offsetLeft = 0, int offsetTop = 0) {
        var screenWidth = SystemParameters.PrimaryScreenWidth;
        var screenHeight = SystemParameters.PrimaryScreenHeight;

        var windowWidth = Width;
        var windowHeight = Height;

        var centerX = (screenWidth / 2) - (windowWidth / 2);
        var centerY = (screenHeight / 2) - (windowHeight / 2);

        Left = centerX + offsetLeft;
        Top = centerY + offsetTop;
    }
}
