using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;

namespace MetricServer.AspNetCore
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseMetricsServer(this IWebHostBuilder builder, Action<IMetricServerOptionsConfiguration> configure = null)
        {
            return builder.ConfigureServices(services => services.UseMetricsServer(configure));
        }

        public static IServiceCollection UseMetricsServer(this IServiceCollection services, Action<IMetricServerOptionsConfiguration> configure = null)
        {
            var builder = new MetricServerOptionsConfiguration(services);
            configure?.Invoke(builder);

            services.AddOptions<MetricServerOptions>().Configure(builder.Apply);
            services.AddHostedService<MetricHostedService>();

            return services;
        }
    }

    internal class HttpMetricsStartupFilter : IStartupFilter
    {
        private readonly ILogger<HttpMetricsStartupFilter> _logger;
        private readonly MetricServerOptions _options;

        public HttpMetricsStartupFilter(ILogger<HttpMetricsStartupFilter> logger, IOptionsMonitor<MetricServerOptions> options)
        {
            _logger = logger;
            _options = options.CurrentValue;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                _logger.LogDebug("Configure:UseHttpMetrics");

                app.UseHttpMetrics(_options.HttpMetrics);

                next(app);
            };
        }
    }
}