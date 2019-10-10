namespace MetricServer.AspNetCore
{
    public interface IMetricOptionsBuilder
    {
        IMetricOptionsBuilder WithHost(string host);
        IMetricOptionsBuilder WithPort(int port);
        IMetricOptionsBuilder EnableHttpMetrics();
    }

    internal class MetricOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableHttpMetrics { get; set; }
    }
}