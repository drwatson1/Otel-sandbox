using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add OpenTelemetry services
builder.Services.AddOpenTelemetryMetrics(builder =>
{
    builder
        .AddMeter("otel-webapi-service")
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
