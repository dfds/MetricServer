using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Prometheus.HttpMetrics;
using Xunit;

namespace MetricServer.AspNetCore.Tests
{
    public class TestMetricServerOptionsConfiguration
    {
        [Fact]
        public void Can_set_host()
        {
            var services = new ServiceCollection();
            var spy = new MetricServerOptions();
            var sut = new MetricServerOptionsConfiguration(services);
            sut.WithHost("foo");

            sut.Apply(spy);

            Assert.Equal("foo", spy.Host);
        }

        [Fact]
        public void Can_set_port()
        {
            var services = new ServiceCollection();
            var spy = new MetricServerOptions();
            var sut = new MetricServerOptionsConfiguration(services);
            sut.WithPort(8888);

            sut.Apply(spy);

            Assert.Equal(8888, spy.Port);
        }

        [Fact]
        public void Can_enable_http_metrics()
        {
            var services = new ServiceCollection();
            var spy = new MetricServerOptions();
            var sut = new MetricServerOptionsConfiguration(services);
            sut.EnableHttpMetrics();

            sut.Apply(spy);

            var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == typeof(IStartupFilter));

            Assert.NotNull(serviceDescriptor);
            Assert.Equal(typeof(HttpMetricsStartupFilter), serviceDescriptor.ImplementationType);
            Assert.Equal(ServiceLifetime.Singleton, serviceDescriptor.Lifetime);
        }

        [Fact]
        public void Can_override_default_http_metrics()
        {
            var services = new ServiceCollection();
            var spy = new MetricServerOptions();
            var sut = new MetricServerOptionsConfiguration(services);
            Action<HttpMiddlewareExporterOptions> stub = x => { };
            sut.EnableHttpMetrics(stub);

            sut.Apply(spy);

            Assert.Equal(stub, spy.HttpMetrics);
        }
    }
}