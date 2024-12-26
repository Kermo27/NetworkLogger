using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetworkLogger.Configuration;
using NetworkLogger.Logging;

namespace NetworkLogger.Services;

public class NetworkMonitoringService : BackgroundService
{
    private readonly ISpeedTestService _speedTestService;
    private readonly IPingService _pingService;
    private readonly IMetricsLogger _metricsLogger;
    private readonly ILogger<NetworkMonitoringService> _logger;
    private readonly IOptions<MonitoringOptions> _options;

    public NetworkMonitoringService(
        ISpeedTestService speedTestService, 
        IPingService pingService, 
        IMetricsLogger metricsLogger, 
        ILogger<NetworkMonitoringService> logger, 
        IOptions<MonitoringOptions> options)
    {
        _speedTestService = speedTestService;
        _pingService = pingService;
        _metricsLogger = metricsLogger;
        _logger = logger;
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var pingMetrics = await _pingService.MeasurePingAsync(_options.Value.PingTarget);
                await _metricsLogger.LogPingMetricsAsync(pingMetrics);

                // var speedMetrics = await _speedTestService.MeasureSpeedAsync(_options.Value.SpeedTestUrl);
                // await _metricsLogger.LogSpeedMetricsAsync(speedMetrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during network monitoring cycle");
            }

            await Task.Delay(TimeSpan.FromMinutes(_options.Value.MonitoringIntervalMinutes), stoppingToken);
        }
    }
}