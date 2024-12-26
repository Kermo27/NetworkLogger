using System.Diagnostics;
using Microsoft.Extensions.Logging;
using NetworkLogger.Models;

namespace NetworkLogger.Services;

public class SpeedTestService : ISpeedTestService
{
    private readonly ILogger<SpeedTestService> _logger;
    private readonly HttpClient _httpClient;

    public SpeedTestService(ILogger<SpeedTestService> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
    }

    public async Task<SpeedMetrics> MeasureSpeedAsync(string url)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength ?? 0;
            stopwatch.Stop();

            var speedMbps = (totalBytes * 8) / (stopwatch.Elapsed.TotalMilliseconds * 1_000_000);
            
            return new SpeedMetrics(DateTime.Now, speedMbps);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to measure download speed");
            throw;
        }
    }
}