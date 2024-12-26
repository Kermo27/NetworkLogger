using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using NetworkLogger.Models;

namespace NetworkLogger.Services;

public class PingService : IPingService
{
    private readonly ILogger<PingService> _logger;

    public PingService(ILogger<PingService> logger)
    {
        _logger = logger;
    }

    private const int SentPackets = 5;
    
    public async Task<PingMetrics> MeasurePingAsync(string target)
    {
        using var ping = new Ping();
        var lostPackets = 0;
        var totalPing = 0.0;

        for (var i = 0; i < SentPackets; i++)
        {
            try
            {
                var reply = await ping.SendPingAsync(target, 1000);
                if (reply.Status == IPStatus.Success)
                    totalPing += reply.RoundtripTime;
                else
                    lostPackets++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ping attempt failed");
                lostPackets++;
            }
        }

        var successfulPings = SentPackets - lostPackets;
        var avgPing = successfulPings > 0 ? (int)Math.Round(totalPing / successfulPings) : 0;
        var packetLoss = (double)lostPackets / SentPackets * 100;

        return new PingMetrics(DateTime.Now, avgPing, packetLoss);
    }
}