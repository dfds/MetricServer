# MetricServer

## MetricServer.AspNetCore

A thin veneer on top of [prometheus-net.AspNetCore](https://github.com/prometheus-net/prometheus-net) for bootstrapping a metrics server on a different port than the main ASP.NET Core application.

### Installation

.NET CLI

```bash
> dotnet add package MetricServer.AspNetCore
```

Package Manager

```
PM> Install-Package MetricServer.AspNetCore
```

### Usage

Either add `UseMetricServer()` directly to `IWebHostBuilder` in the `Program.cs`:

```csharp
    // ...
    public class Program
    {
        public static int Main(string[] args)
        {
            // ...
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseMetricsServer()         // ADD THIS LINE
                .UseStartup<Startup>();
    }
    // ...
 ```

or use `UseMetricsServer()` on the `IServiceCollection` in the `Startup.cs`:

```csharp
    // ...
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // ...
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // ...

            services.AddMetricsServer();    // ADD THIS LINE
        }

        // ...
    }
    // ...
```

### Configuration

By default the Host is `0.0.0.0` and the Port is `8080`. To enable capture of HTTP metrics use the `EnableHttpMetrics()` as in the snippet below:

```csharp
    // ...
    public class Program
    {
        public static int Main(string[] args)
        {
            // ...
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseMetricsServer(opt => 
                {
                    opt.WithHost("localhost");
                    opt.WithPort(5050);
                    opt.EnableHttpMetrics();

                })
                .UseStartup<Startup>();
    }
    // ...
 ```
