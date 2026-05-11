using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace AWMicroservices.SalesOrders.API.Extensions;

public static class LoggingExtensions
{
  /// <summary>
  /// Registers Serilog as the application logger with rolling daily file and 7-day retention.
  /// </summary>
  public static IHostBuilder AddSerilogLogging(this IHostBuilder hostBuilder)
  {
    return hostBuilder.UseSerilog((context, services, config) =>
    {
      var logDir = Path.Combine(AppContext.BaseDirectory, "logs");
      Directory.CreateDirectory(logDir);

      config
              .ReadFrom.Configuration(context.Configuration)
              .ReadFrom.Services(services)
              .Enrich.FromLogContext()
              .Enrich.WithEnvironmentName()
              .Enrich.WithMachineName()
              .WriteTo.File(
                  formatter: new CompactJsonFormatter(),
                  path: Path.Combine(logDir, "log-.json"),
                  rollingInterval: RollingInterval.Day,
                  retainedFileCountLimit: 7,
                  shared: false)
              .WriteTo.Console(outputTemplate:
                  "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}");
    });
  }
}
