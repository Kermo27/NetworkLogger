using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetworkLogger.Configuration;
using NetworkLogger.Models;

namespace NetworkLogger.Logging;

public class CsvMetricsLogger : IMetricsLogger
{
    private readonly ILogger<CsvMetricsLogger> _logger;
    private readonly IOptions<MonitoringOptions> _options;

    public CsvMetricsLogger(ILogger<CsvMetricsLogger> logger, IOptions<MonitoringOptions> options)
    {
        _logger = logger;
        _options = options;
        InitializeLogFiles();
    }

    private void InitializeLogFiles()
    {
        if (!File.Exists(_options.Value.PingLogPath))
            File.WriteAllText(_options.Value.PingLogPath, "Timestamp,Avg_Ping (ms),Packet_Loss (%)\n");
        
        if (!File.Exists(_options.Value.SpeedLogPath))
            File.WriteAllText(_options.Value.SpeedLogPath, "Timestamp,Download_Speed (Mbps)\n");
    }

    public async Task LogPingMetricsAsync(PingMetrics metrics)
    {
        var logEntry = $"{metrics.Timestamp:yyyy-MM-dd HH:mm:ss},{metrics.AveragePing:F2},{metrics.PacketLoss:F2}\n";
        await File.AppendAllTextAsync(_options.Value.PingLogPath, logEntry);
        _logger.LogInformation("Logged ping metrics: {Metrics}", metrics);
    }

    public async Task LogSpeedMetricsAsync(SpeedMetrics metrics)
    {
        var logEntry = $"{metrics.Timestamp:yyyy-MM-dd HH:mm:ss},{metrics.DownloadSpeedMbps:F2}\n";
        await File.AppendAllTextAsync(_options.Value.SpeedLogPath, logEntry);
        _logger.LogInformation("Logged speed metrics: {Metrics}", metrics);
    }
}