using OpenTelemetry.Metrics;
using otel_webapi_service;

var builder = WebApplication.CreateBuilder(args);

/*
public static class ServiceCollectionExtensions
{
    public static void AddFactory<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.AddTransient<TService, TImplementation>();
        services.AddSingleton<Func<TService>>(x => () => x.GetService<TService>());
    }
}
*/

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var metrics = new Metrics();
builder.Services.AddSingleton<Metrics>(metrics);
builder.Services.AddTransient<WorkerFactory>();

// Add OpenTelemetry services
builder.Services.AddOpenTelemetryMetrics(builder =>
{
    builder
        .AddMeter(metrics.Meter.Name)
        //.AddView(instrumentName: metrics.Workers.WorkTimeHist.Name, new ExplicitBucketHistogramConfiguration() { Boundaries = new double[] { 10 } })
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddPrometheusExporter();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure Prometheus exporter
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapControllers();

app.Run();
