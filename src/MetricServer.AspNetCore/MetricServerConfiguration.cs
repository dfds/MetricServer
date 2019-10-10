using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Prometheus.HttpMetrics;

namespace MetricServer.AspNetCore
{
    public interface IMetricServerOptionsConfiguration
    {
        IMetricServerOptionsConfiguration WithHost(string host);
        IMetricServerOptionsConfiguration WithPort(int port);
        IMetricServerOptionsConfiguration EnableHttpMetrics(Action<HttpMiddlewareExporterOptions> configure = null);
    }

    internal class MetricServerOptionsConfiguration : IMetricServerOptionsConfiguration
    {
        private static readonly Action<HttpMiddlewareExporterOptions> NoOpHttpMetrics = options => { };

        private readonly IList<Action<MetricServerOptions>> _configurations = new List<Action<MetricServerOptions>>();
        private readonly IServiceCollection _services;

        public MetricServerOptionsConfiguration(IServiceCollection services)
        {
            _services = services;
        }

        public IMetricServerOptionsConfiguration WithHost(string host)
        {
            return AddConfiguration(options => options.Host = host);
        }

        private IMetricServerOptionsConfiguration AddConfiguration(Action<MetricServerOptions> action)
        {
            _configurations.Add(action);
            return this;
        }

        public IMetricServerOptionsConfiguration WithPort(int port)
        {
            return AddConfiguration(options => options.Port = port);
        }

        public IMetricServerOptionsConfiguration EnableHttpMetrics(Action<HttpMiddlewareExporterOptions> configure = null)
        {
            _services.AddSingleton<IStartupFilter, HttpMetricsStartupFilter>();
            return AddConfiguration(options => options.HttpMetrics = configure ?? NoOpHttpMetrics);
        }

        public void Apply(MetricServerOptions options)
        {
            foreach (var configuration in _configurations)
            {
                configuration(options);
            }
        }
    }
}