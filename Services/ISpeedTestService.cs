using NetworkLogger.Models;

namespace NetworkLogger.Services;

public interface ISpeedTestService
{
    Task<SpeedMetrics> MeasureSpeedAsync(string url);
}