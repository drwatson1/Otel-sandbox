namespace otel_webapi_service
{
    public class WorkerFactory
    {
        Metrics Metrics { get; }

        public WorkerFactory(Metrics metrics)
        {
            Metrics = metrics;
        }

        public Worker CreateWorker(string name)
        {
            return new Worker(Metrics, name);
        }
    }
}
