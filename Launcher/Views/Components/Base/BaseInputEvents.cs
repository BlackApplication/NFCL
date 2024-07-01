using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Launcher.Views.Components.Base;

public class BaseInputEvents : UserControl {

    #region Variables

    private bool IsFocus;

    protected Border border = null!;
    protected Border strip = null!;

    #endregion

    #region Inputs Events

    protected void Focus(object sender, RoutedEventArgs e) {
        IsFocus = true;
    }

    protected void UnFocus(object sender, RoutedEventArgs e) {
        IsFocus = false;
        UpdateBorderOpacity(false);
    }

    protected void OverMouse(object sender, MouseEventArgs e) {
        UpdateBorderOpacity(true);
    }

    protected void LeaveMouse(object sender, MouseEventArgs e) {
        if (!IsFocus) {
            UpdateBorderOpacity(false);
        }
    }

    protected void UpdateBorderOpacity(bool isActive) {
        border.Opacity = isActive ? 1 : 0.7;
        strip.Opacity = isActive ? 1 : 0.7;
    }

    #endregion
}
