using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;

namespace MetricServer.AspNetCore
{
    public class MetricHostedService : IHostedService
    {
        private readonly ILogger<MetricHostedService> _logger;
        private readonly IOptionsMonitor<MetricServerOptions> _options;

        private IMetricServer _metricServer;

        public MetricHostedService(ILogger<MetricHostedService> logger, IOptionsMonitor<MetricServerOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Staring metric server on {Host}:{Port}", _options.CurrentValue.Host, _options.CurrentValue.Port);
            _metricServer = new KestrelMetricServer(_options.CurrentValue.Host, _options.CurrentValue.Port).Start();
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using (_metricServer)
            {
                _logger.LogDebug("Shutting down metric server");
                await _metricServer.StopAsync();
                _logger.LogDebug("Done shutting down metric server");
            }
        }
    }
}