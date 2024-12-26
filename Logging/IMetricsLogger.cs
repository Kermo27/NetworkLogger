using NetworkLogger.Models;

namespace NetworkLogger.Logging;

public interface IMetricsLogger
{
    Task LogPingMetricsAsync(PingMetrics metrics);
    Task LogSpeedMetricsAsync(SpeedMetrics metrics);
}