using Core.ViewModels;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

namespace Launcher.Views.Pages;

[MvxContentPresentation]
[MvxViewFor(typeof(DashboardViewModel))]
public partial class Dashboard : MvxWpfView {
    public Dashboard() {
        InitializeComponent();
    }
}
