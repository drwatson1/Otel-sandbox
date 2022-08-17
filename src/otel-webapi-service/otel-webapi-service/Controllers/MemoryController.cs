using Microsoft.AspNetCore.Mvc;

namespace otel_webapi_service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MemoryController : ControllerBase
    {
        static private List<byte[]> memoryBlocks = new List<byte[]>();

        [HttpPost("Allocate")]
        public IActionResult Allocate(int? bytes, int? kbytes, int? mbytes)
        {
            int blockSize = (bytes ?? 0) + (kbytes ?? 0) * 1024 + (mbytes ?? 0) * 1024 * 1024;

            var block = new byte[blockSize];
            memoryBlocks.Add(block);

            return Ok($"Allocated {blockSize} bytes. Blocks count is {memoryBlocks.Count}");
        }

        [HttpPost("FreeAll")]
        public IActionResult FreeAll()
        {
            memoryBlocks.Clear();

            return Ok($"Blocks count is {memoryBlocks.Count}");
        }

        [HttpPost("Free")]
        public IActionResult Free(int index)
        {
            if (index >= memoryBlocks.Count)
            {
                return BadRequest($"Index is out of scope. The count of memotry blocks is is {memoryBlocks.Count}");
            }
            memoryBlocks.RemoveAt(index);

            return Ok($"Blocks count is {memoryBlocks.Count}");
        }
    }
}
