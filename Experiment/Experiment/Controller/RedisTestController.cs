using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Experiment;

[ApiController]
[Route("[controller]")]
public class RedisTestController : ControllerBase
{
    private readonly IDatabase _db;

    public RedisTestController(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetValue(string key)
    {
        var value = await _db.StringGetAsync(key);
        return Ok(new { key, value = value.ToString() });
    }
}