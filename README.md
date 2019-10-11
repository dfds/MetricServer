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

or use `AddMetricServer()` on the `IServiceCollection` in the `Startup.cs`:

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
