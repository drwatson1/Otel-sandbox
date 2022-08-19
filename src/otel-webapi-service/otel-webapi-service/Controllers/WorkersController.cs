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

        [HttpPost("Run")]
        public IActionResult RunWorker(string name)
        {
            var worker = Factory.CreateWorker(name);
            Task.Run(() => worker.Run());

            return Ok();
        }
    }
}
