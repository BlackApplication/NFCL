using Models.Enums;
using System.Windows;
using System.Windows.Controls;

namespace Launcher.Views.Components;

public partial class PageFooter : UserControl {
    public string Version {
        get => (string)GetValue(VersionDependency);
        set => SetValue(VersionDependency, value);
    }

    public static readonly DependencyProperty VersionDependency =
        DependencyProperty.Register("Version", typeof(string), typeof(PageFooter), new PropertyMetadata("None"));

    public PageFooter() {
        InitializeComponent();
    }
}
