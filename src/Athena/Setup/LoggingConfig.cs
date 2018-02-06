using Serilog;
using Serilog.Events;

namespace Athena.Setup
{
    public static class LoggingConfig
    {
        public static void SetupSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole()
                .CreateLogger();
        }
    }
}