namespace NetworkLogger.Models;

public record PingMetrics(DateTime Timestamp, int AveragePing, double PacketLoss);