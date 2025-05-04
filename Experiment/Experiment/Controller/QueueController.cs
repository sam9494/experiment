using Experiment.Services;
using Microsoft.AspNetCore.Mvc;

namespace Experiment.Controller;

[ApiController]
[Route("[controller]")]
public class QueueController(RedisQueueService queueService) : ControllerBase
{
    [HttpPost("enqueue")]
    public async Task<IActionResult> Enqueue(string value)
    {
        await queueService.EnqueueAsync("queue:pending", value);
        return Ok($"Enqueued: {value}");
    }
}