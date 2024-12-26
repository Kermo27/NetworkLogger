namespace NetworkLogger.Configuration;

public record MonitoringOptions
{
    public string PingTarget { get; init; } = "google.com";
    public string SpeedTestUrl { get; init; } = "https://ipv4.download.thinkbroadband.com/100MB.zip";
    public int MonitoringIntervalMinutes { get; init; } = 1;
    public string PingLogPath { get; init; } = "ping_log.csv";
    public string SpeedLogPath { get; init; } = "speed_log.csv";
}