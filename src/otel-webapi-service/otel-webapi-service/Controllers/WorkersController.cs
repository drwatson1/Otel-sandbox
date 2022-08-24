using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace otel_webapi_service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        WorkerFactory Factory { get; }

        public WorkersController(WorkerFactory factory)
        {
            Factory = factory;
        }

        /// <summary>
        /// Run a Worker with the given name. The Worker works in the background for the 0 - 120 seconds and than finishes.
        /// </summary>
        /// <param name="name"></param>
        [HttpPost("Run")]
        public IActionResult RunWorker(string name)
        {
            var worker = Factory.CreateWorker(name);
            Task.Run(() => worker.Run());

            return Ok();
        }
    }
}
