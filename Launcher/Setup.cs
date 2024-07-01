using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Wpf.Core;
using Serilog;
using Serilog.Extensions.Logging;

namespace Launcher;

public class Setup : MvxWpfSetup<Core.App> {
    protected override ILoggerFactory? CreateLogFactory() {
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

        return new SerilogLoggerFactory();
    }

    protected override ILoggerProvider? CreateLogProvider() {
        return new SerilogLoggerProvider();
    }
}
