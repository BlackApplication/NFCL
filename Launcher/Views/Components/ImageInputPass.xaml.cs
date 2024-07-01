using Launcher.Views.Components.Base;
using System.Windows;
using System.Windows.Data;

namespace Launcher.Views.Components;

public partial class ImageInputPass : BaseInputEvents {
    public string ImagePath {
        get => (string)GetValue(ImagePathDependency);
        set => SetValue(ImagePathDependency, value);
    }

    public static readonly DependencyProperty ImagePathDependency =
        DependencyProperty.Register("ImagePath", typeof(string), typeof(ImageInputPass), new PropertyMetadata(string.Empty));

    private bool _isPasswordChanging;

    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register("Password", typeof(string), typeof(ImageInputPass),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                PasswordPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

    private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is ImageInputPass passwordBox) {
            passwordBox.UpdatePassword();
        }
    }

    public string Password {
        get { return (string)GetValue(PasswordProperty); }
        set { SetValue(PasswordProperty, value); }
    }

    public ImageInputPass() {
        InitializeComponent();
        border = Border;
        strip = Strip;
    }

    private void UpdatePassword() {
        if (!_isPasswordChanging) {
            passwordBox.Password = Password;
        }
    }

    private void PasswordChange(object sender, RoutedEventArgs e) {
        _isPasswordChanging = true;
        Password = passwordBox.Password;
        _isPasswordChanging = false;
    }
}
