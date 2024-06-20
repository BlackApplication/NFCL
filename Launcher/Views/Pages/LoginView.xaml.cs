using Core.ViewModels;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Launcher.Views.Pages;

[MvxContentPresentation]
[MvxViewFor(typeof(LoginViewModel))]
public partial class LoginView : MvxWpfView {
    private bool InputEmailIsFocus;

    private bool InputPassIsFocus;

    public LoginView() {
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

    #region Inputs Events

    private void InputEmailFocus(object sender, RoutedEventArgs e) {
        InputEmailIsFocus = true;
    }

    private void InputEmailUnFocus(object sender, RoutedEventArgs e) {
        InputEmailIsFocus = false;
        UpdateBorderOpacity(InputEmailBorder, InputEmailStrip,false);
    }

    private void InputPassFocus(object sender, RoutedEventArgs e) {
        InputPassIsFocus = true;
    }

    private void InputPassUnFocus(object sender, RoutedEventArgs e) {
        InputPassIsFocus = false;
        UpdateBorderOpacity(InputPassBorder, InputPassStrip, false);
    }

    private void InputEmailOverMouse(object sender, MouseEventArgs e) {
        UpdateBorderOpacity(InputEmailBorder, InputEmailStrip, true);
    }

    private void InputEmailLeaveMouse(object sender, MouseEventArgs e) {
        if (!InputEmailIsFocus) {
            UpdateBorderOpacity(InputEmailBorder, InputEmailStrip, false);
        }
    }

    private void InputPassOverMouse(object sender, MouseEventArgs e) {
        UpdateBorderOpacity(InputPassBorder, InputPassStrip, true);
    }

    private void InputPassLeaveMouse(object sender, MouseEventArgs e) {
        if (!InputPassIsFocus) {
            UpdateBorderOpacity(InputPassBorder, InputPassStrip, false);
        }
    }

    private void UpdateBorderOpacity(Border inputBorder, Border inputStrip , bool isActive) {
        inputBorder.Opacity = isActive ? 1 : 0.7;
        inputStrip.Opacity = isActive ? 1 : 0.7;
    }

    #endregion
}
