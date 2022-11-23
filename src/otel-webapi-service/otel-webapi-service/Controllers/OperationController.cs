using Microsoft.AspNetCore.Mvc;

namespace otel_webapi_service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        Metrics Metrics { get; }
        static readonly Random Random = new Random();

        public OperationController(Metrics metrics)
        {
            Metrics = metrics;
        }

        [HttpGet("Run")]
        public string Run()
        {
            // Emulate operations

            for(int i = 0; i < 100; i++)
            {
                var rnd = RandNormal(500, 200);
                Metrics.OperationDuration.Record((long)rnd);
            }

            return "Finished";
        }

        double RandNormal(double mean, double stdDev)
        {
            double u1 = 1.0 - Random.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - Random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                         mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

            return randNormal;
        }
    }
}
