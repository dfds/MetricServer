using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace MetricServer.AspNetCore
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseMetricsServer(this IWebHostBuilder builder, Action<IMetricOptionsBuilder> buildOptions = null)
        {
            return builder.ConfigureServices(services => services.UseMetricsServer(buildOptions));
        }

        public static IWebHostBuilder UseMetricsServer(this IWebHostBuilder builder, string host = MetricServerDefault.DefaultHost, int port = MetricServerDefault.DefaultPort)
        {
            return builder.ConfigureServices(services => services.UseMetricsServer(host, port));
        }

        public static IServiceCollection UseMetricsServer(this IServiceCollection services, string host = MetricServerDefault.DefaultHost, int port = MetricServerDefault.DefaultPort)
        {
            return services.UseMetricsServer(builder => { builder.WithHost(host).WithPort(port); });
        }

        public static IServiceCollection UseMetricsServer(this IServiceCollection services, Action<IMetricOptionsBuilder> buildOptions = null)
        {
            var builder = new MetricOptionsBuilder();
            buildOptions?.Invoke(builder);
            var serverOptions = builder.Build();

            if (serverOptions.EnableHttpMetrics)
            {
                services.AddSingleton<IStartupFilter, HttpMetricsStartupFilter>();
            }

            services.AddOptions<MetricServerOptions>().Configure(options =>
            {
                options.Host = serverOptions.Host;
                options.Port = serverOptions.Port;
            });

            services.AddHostedService<MetricHostedService>();

            return services;
        }

        private class HttpMetricsStartupFilter : IStartupFilter
        {
            public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
            {
                return app =>
                {
                    app.UseHttpMetrics(options =>
                    {
                        //Metrics.CreateGauge("http_requests_in_progress", "The number of requests currently in progress in the ASP.NET Core pipeline.");
                        options.InProgress.Enabled = false;

                        //Metrics.CreateCounter("http_requests_received_total", "Provides the count of HTTP requests that have been processed by the ASP.NET Core pipeline.", HttpRequestLabelNames.All);
                        options.RequestCount.Counter = Metrics.CreateCounter("control_tower_request_count_total", "", "code", "method", "controller", "action");

                        //Metrics.CreateHistogram("http_request_duration_seconds", "The duration of HTTP requests processed by an ASP.NET Core application.",
                        //    new HistogramConfiguration
                        //    {
                        //        Buckets = Histogram.ExponentialBuckets(0.001, 2.0, 16),
                        //        LabelNames = HttpRequestLabelNames.All
                        //    }
                        //);

                        options.RequestDuration.Histogram = Metrics.CreateHistogram("control_tower_request_duration_seconds", "",
                            new HistogramConfiguration
                            {
                                Buckets = Histogram.LinearBuckets(1, 1, 64),
                                LabelNames = new[] {"code", "method"}
                            }
                        );
                    });

                    next(app);
                };
            }
        }
    }
}