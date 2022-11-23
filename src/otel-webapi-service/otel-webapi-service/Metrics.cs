using System.Diagnostics.Metrics;

namespace otel_webapi_service
{
    public class Metrics
    {
        public Meter Meter { get; } = new Meter("otel-webapi-service", "1.0");

        public class WorkerMetrics
        {
            public WorkerMetrics(Meter meter)
            {
                ActiveInstances = meter.CreateCounter<long>("otel-workers-active-instances-total", null, "Number of active workers");
                Count = meter.CreateCounter<long>("otel-workers-total", null, "Number of created workers");
            }

            public Counter<long> Count { get; }
            public Counter<long> ActiveInstances { get;}
        }

        public WorkerMetrics Workers { get; }
        public Histogram<long> OperationDuration { get; }

        public Metrics()
        {
            Workers = new WorkerMetrics(Meter);
            OperationDuration = Meter.CreateHistogram<long>("otel-operation-duration","ms", "Operation duration");
        }
    }
}
