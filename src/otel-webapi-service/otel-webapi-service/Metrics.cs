using System.Diagnostics.Metrics;

namespace otel_webapi_service
{
    public class Metrics
    {
        public Meter Meter { get; } = new Meter("otel.webapi.service", "1.0");

        public class WorkerMetrics
        {
            public WorkerMetrics(Meter meter)
            {
                ActiveInstances = meter.CreateCounter<long>("workers.active_count", "items", "Number of active workers");
                Count = meter.CreateCounter<long>("workers.count", "items",  "Number of created workers");
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
