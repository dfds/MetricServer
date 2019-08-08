namespace MetricServer.AspNetCore
{
    public class MetricServerOptions
    {
        public const string DefaultHost = "0.0.0.0";
        public const int DefaultPort = 8080;

        public string Host { get; set; }
        public int Port { get; set; }
    }
}