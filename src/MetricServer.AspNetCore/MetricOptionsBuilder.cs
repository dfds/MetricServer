namespace MetricServer.AspNetCore
{
    internal class MetricOptionsBuilder : IMetricOptionsBuilder
    {
        private string _host = MetricServerDefault.DefaultHost;
        private int _port = MetricServerDefault.DefaultPort;
        private bool _enableHttpMetrics;

        public IMetricOptionsBuilder WithHost(string host)
        {
            _host = host;
            return this;
        }

        public IMetricOptionsBuilder WithPort(int port)
        {
            _port = port;
            return this;
        }

        public IMetricOptionsBuilder EnableHttpMetrics()
        {
            _enableHttpMetrics = true;
            return this;
        }

        public MetricOptions Build()
        {
            return new MetricOptions
            {
                Host = _host,
                Port = _port,
                EnableHttpMetrics = _enableHttpMetrics
            };
        }
    }
}