using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetworkLogger.Configuration;
using NetworkLogger.Logging;
using NetworkLogger.Services;

namespace NetworkLogger;

public class Program
{
    public static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<NetworkMonitoringService>();
                services.Configure<MonitoringOptions>(
                    context.Configuration.GetSection(nameof(MonitoringOptions)));
                services.AddTransient<ISpeedTestService, SpeedTestService>();
                services.AddTransient<IPingService, PingService>();
                services.AddTransient<IMetricsLogger, CsvMetricsLogger>();
                services.AddOptions();
            })
            .Build()
            .RunAsync();
    }
}