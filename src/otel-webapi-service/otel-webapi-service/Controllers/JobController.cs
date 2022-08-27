using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace otel_webapi_service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        static readonly Random random = new();

        /// <summary>
        /// The action synchronously runs a task for 0 - 500 msec before returns an answer
        /// </summary>
        [HttpGet("RunLongJob")]
        public IActionResult RunLongJob()
        {
            var delay = random.Next(500);

            Thread.Sleep(delay);

            return Ok($"The job takes {delay} msec");
        }

        /// <summary>
        /// The action synchronously runs a task for 600 - 1000 msec before returns an answer
        /// </summary>
        /// <returns></returns>
        [HttpGet("RunVeryLongJob")]
        public IActionResult RunVeryLongJob()
        {
            var delay = random.Next(600, 1000);

            Thread.Sleep(delay);

            return Ok($"The job takes {delay} msec");
        }
    }
}
