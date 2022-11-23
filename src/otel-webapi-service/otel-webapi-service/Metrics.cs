using System.Diagnostics.Metrics;

namespace otel_webapi_service
{
    public class Metrics
    {
        public Meter Meter { get; } = new Meter("otel_webapi_service", "1.0");

        public class WorkerMetrics
        {
            public WorkerMetrics(Meter meter)
            {
                ActiveInstances = meter.CreateCounter<long>("otel_workers_active_instances_total", "items", "Number of active workers");
                Count = meter.CreateCounter<long>("otel_workers_total", "items",  "Number of created workers");
            }

            public Counter<long> Count { get; }
            public Counter<long> ActiveInstances { get;}
        }

        public WorkerMetrics Workers { get; }

        public Metrics()
        {
            Workers = new WorkerMetrics(Meter);
        }
    }
}
