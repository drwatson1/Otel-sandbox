using System.Diagnostics;

namespace otel_webapi_service
{
    public class Worker
    {
        private Metrics Metrics { get; }
        private TagList TagList { get; }

        public Worker (Metrics metrics, string name)
        {
            Metrics = metrics;
            TagList = new TagList
            {
                new("name", name)
            };
        }

        /// <summary>
        /// Run the worker for the 0-12 sec
        /// </summary>
        public async Task Run()
        {
            await Task.Run(async () =>
            {
                Metrics.Workers.ActiveInstances.Add(1, TagList);
                Metrics.Workers.Count.Add(1, TagList);

                var sec = Random.Shared.Next(120);
                await Task.Delay(sec * 1000);
            });
            Metrics.Workers.ActiveInstances.Add(-1, TagList);
        }
    }
}
