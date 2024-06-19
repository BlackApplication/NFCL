using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Wpf.Core;

namespace Launcher;

public class Setup : MvxWpfSetup<Core.App> {
    protected override ILoggerFactory? CreateLogFactory() {
        return null;
    }

    protected override ILoggerProvider? CreateLogProvider() {
        return null;
    }
}
