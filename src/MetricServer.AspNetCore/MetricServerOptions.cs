using System;
using Prometheus.HttpMetrics;

namespace MetricServer.AspNetCore
{
    public class MetricServerOptions
    {
        public const string DefaultHost = "0.0.0.0";
        public const int DefaultPort = 8080;

        public string Host { get; set; } = DefaultHost;
        public int Port { get; set; } = DefaultPort;
        public Action<HttpMiddlewareExporterOptions> HttpMetrics { get; set; }
    }
}