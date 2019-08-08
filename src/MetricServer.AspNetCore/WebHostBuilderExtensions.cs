using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MetricServer.AspNetCore
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseMetricsServer(this IWebHostBuilder builder, string host = MetricServerOptions.DefaultHost, int port = MetricServerOptions.DefaultPort)
        {
            return builder.ConfigureServices(services => services.AddMetricsServer(host, port));
        }

        public static IServiceCollection AddMetricsServer(this IServiceCollection services, string host = MetricServerOptions.DefaultHost, int port = MetricServerOptions.DefaultPort)
        {
            services.AddOptions<MetricServerOptions>().Configure(options =>
            {
                options.Host = host;
                options.Port = port;
            });
            services.AddHostedService<MetricHostedService>();
            return services;
        }
    }
}