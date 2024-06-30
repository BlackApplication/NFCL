using Launcher.Views.Components.Base;
using System.Windows;
using System.Windows.Data;

namespace Launcher.Views.Components;

public partial class ImageInput : BaseInputEvents {
    public string ImagePath {
        get => (string)GetValue(ImagePathDependency);
        set => SetValue(ImagePathDependency, value);
    }

    public static readonly DependencyProperty ImagePathDependency =
        DependencyProperty.Register("ImagePath", typeof(string), typeof(ImageInput), new PropertyMetadata(string.Empty));

    private bool _isInputChanging;

    public static readonly DependencyProperty InputTextProperty =
        DependencyProperty.Register("InputText", typeof(string), typeof(ImageInput),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                InputPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

    private static void InputPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is ImageInput inputBox) {
            inputBox.UpdateLayout();
        }
    }

    public string InputText {
        get { return (string)GetValue(InputTextProperty); }
        set { SetValue(InputTextProperty, value); }
    }

    public ImageInput() {
        InitializeComponent();
        border = Border;
        strip = Strip;
    }

    private void UpdateInput() {
        if (!_isInputChanging) {
            InputBox.Text = InputText;
        }
    }

    private void InputChange(object sender, System.Windows.Controls.TextChangedEventArgs e) {
        _isInputChanging = true;
        InputText = InputBox.Text;
        _isInputChanging = false;
    }
}
