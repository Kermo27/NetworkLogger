using NetworkLogger.Models;

namespace NetworkLogger.Services;

public interface IPingService
{
    Task<PingMetrics> MeasurePingAsync(string target);
}