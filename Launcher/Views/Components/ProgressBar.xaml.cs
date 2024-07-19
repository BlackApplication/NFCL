using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Launcher.Views.Components;

public partial class ProgressBar : UserControl, INotifyPropertyChanged {
    public string LoadText {
        get => (string)GetValue(LoadTextDependency);
        set => SetValue(LoadTextDependency, value);
    }

    public static readonly DependencyProperty LoadTextDependency =
        DependencyProperty.Register("LoadText", typeof(string), typeof(ProgressBar),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnLoadTextChanged));

    private static void OnLoadTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is ProgressBar progressBar) {
            progressBar.UpdateLoadText();
        }
    }

    public int TextSize {
        get => (int)GetValue(TextSizeDependency);
        set => SetValue(TextSizeDependency, value);
    }

    public static readonly DependencyProperty TextSizeDependency =
        DependencyProperty.Register("TextSize", typeof(int), typeof(ProgressBar),
            new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public int Percent {
        get => (int)GetValue(PercentDependency);
        set => SetValue(PercentDependency, value);
    }

    public static readonly DependencyProperty PercentDependency =
        DependencyProperty.Register("Percent", typeof(int), typeof(ProgressBar),
            new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPercentChanged));

    private static void OnPercentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is ProgressBar progressBar) {
            progressBar.UpdateProgressWidth();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public double ProgressWidth {
        get {
            var width = ProgressBarLine.ActualWidth;
            var progressWidth = width / 100 * Percent;

            return progressWidth;
        }
    }
    

    public ProgressBar() {
        InitializeComponent();
        ProgressBarLine.SizeChanged += (s, e) => UpdateProgressWidth();
    }

    private void UpdateProgressWidth() {
        OnPropertyChanged(nameof(ProgressWidth));
    }

    private void UpdateLoadText() {
        OnPropertyChanged(nameof(LoadText));
    }

    private void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
