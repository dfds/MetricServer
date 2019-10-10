namespace MetricServer.AspNetCore
{
    public static class MetricServerDefault
    {
        public const string DefaultHost = "0.0.0.0";
        public const int DefaultPort = 8080;
    }

    public class MetricServerOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}