# .Net 6 WebAPI Service with OpenTelemetry Metrics Example

## Overview

OpenTelemetry is a collection of tools, APIs, and SDKs. Use it to instrument, generate, collect, and export telemetry data (metrics, logs, and traces) to help you analyze your software's performance and behavior.

If you are not familiar with the OpenTelemetry project, I strongly recommend you thoroughtly read the official documentation on the <https://opentelemetry.io/> to get a deep understanding of underlain concepts.

This repository is focused on metrics only and contains an example of a .Net 6 service instrumented with some useful metrics as well as a bunch of custom metrics. The service can be used as a quick start with the OpenTelemetry. Also, this README contains step-by-step instructions how to add and use OpenTelemetry metrics to your own service.

## Step-by-step Guide

### 1. Create a New Service and Configure OpenTelemetry

You can create a new service or use an existing one.

Add the necessary packages to your project:

```bash
dotnet add package OpenTelemetry --version 1.3.0
dotnet add package OpenTelemetry.Extensions.Hosting --version 1.0.0-rc9.4
dotnet add package OpenTelemetry.Exporter.Prometheus --version 1.3.0-rc.2
```

The first two packages is mandatory and contains core components.

The last added package is `OpenTelemetry.Exporter.Prometheus`. It's a so called `exporter`. Exporters are used to gather and pass all metrics to a backend to collect, store and query them. Typically, we want to use a time-series database as the backend. Of course, you can use console exporter, but we use `Prometheus` (see below) in this example as much more useful one. You can read more about exporters on [this](https://opentelemetry.io/docs/instrumentation/net/exporters/) page.

In your `Program.cs` add the following code to add all necessary services:

```csharp
// Add OpenTelemetry services
builder.Services.AddOpenTelemetryMetrics(builder =>
{
    builder
        .AddMeter("otel-webapi-service")
        .AddPrometheusExporter();
});
```

Here, we add a new `Meter` with `otel-webapi-service` name and all necessary for the exporter. Then enable the Prometheus exporter:

```csharp
// Configure Prometheus exporter
app.UseOpenTelemetryPrometheusScrapingEndpoint();
```

It automatically adds a new endpoint `/metrics` which can be used to get metrics.
Build and start your service, open a browser and navigate to `http://localhost:5000/metrics` to see your service metrics. Note! The port can other than `5000`, so please, don't forget to use a right one.

Do you see anything? No, you don't. This is because we didn't add any metrics. Let's do it.

### 2. Add a Basic Telemetry

Add the following instrumentation package:

```bash
dotnet add package OpenTelemetry.Instrumentation.Runtime --version 1.0.0
```

And then configure metrics:

```csharp
// Add OpenTelemetry services
builder.Services.AddOpenTelemetryMetrics(builder =>
{
    builder
        .AddMeter("otel-webapi-service")
        .AddRuntimeInstrumentation()  // <---- Add this line
        .AddPrometheusExporter();
});
```

Start your service, wait a minute and navigate to `http://localhost:5000/metrics`. Now you see some metrics, automatically gathered for you by the instrumentation library. You'll see something like this:

```bash
# HELP process_runtime_dotnet_gc_allocations_size_bytes Count of bytes allocated on the managed GC heap since the process start. .NET objects are allocated from this heap. Object allocations from unmanaged languages such as C/C++ do not use this heap.
# TYPE process_runtime_dotnet_gc_allocations_size_bytes counter
process_runtime_dotnet_gc_allocations_size_bytes 10566480 1660737250055

# HELP process_runtime_dotnet_thread_pool_threads_count The number of thread pool threads that currently exist.
# TYPE process_runtime_dotnet_thread_pool_threads_count gauge
process_runtime_dotnet_thread_pool_threads_count 9 1660737250055

other metrics....
```

More information about DotNet Runtime instrumentation package your can find in this [GitHub repo](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/main/src/OpenTelemetry.Instrumentation.Runtime).

### 3. Install and Run Prometheus

We will use [Prometheus](https://prometheus.io/) to collect metrics which our service will produce and publish.

#### What is the Prometheus?

It's an open-source time-series database with alerting and query language.

#### Why is the Prometheus?

Basically, the reasons are:

* Simple installation
* Very fast
* Low resource consumption
* Rich query language and integrated data visualization

The alternatives you can find on [this](https://prometheus.io/docs/introduction/comparison/) page.

Let's begin. [Download the latest release](https://prometheus.io/download). Follow the [instruction](https://prometheus.io/docs/introduction/first_steps/) to configure and run it. You can use the simplest possible configuration file as following:

```yaml
global:
  scrape_interval: 15s
  evaluation_interval: 15s

scrape_configs:
  - job_name: "Otel-web-api-service"
    static_configs:
      - targets: ["localhost:5000"]
```

Note the last line with the `localhost:5000` string. This is your service host and port.
Just place it next to Prometheus executable file and run it, then run your service and wait a couple of minutes.

Navigate to `http://localhost:9090` - this is the defaults for the Prometheus, and type `process_runtime_dotnet_gc_allocations_size_bytes` in the query field:

![image](./images/prometheus-1.jpg)

Select `Graph` tab:

![image](./images/prometheus-2.jpg)

If you use a service example from this repo, you can play with `Memory/Allocate`, `Memory/Free` and `Memory/FreeAll` endpoints to allocate and free memory to see how it influences to the plot:

![image](./images/prometheus-3.jpg)
