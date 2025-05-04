using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Experiment;

[ApiController]
[Route("[controller]")]
public class RedisIncrController : ControllerBase
{
    private readonly IDatabase _db;

    public RedisIncrController(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    [HttpPost("incr")]
    public async Task<IActionResult> IncrementCounter(string key)
    {
        // Redis INCR：會將 key 對應的值 +1，若 key 不存在，會從 0 開始
        long result = await _db.StringIncrementAsync(key);
        return Ok(new { key, value = result });
    }
    
    [HttpPost("incrby")]
    public async Task<IActionResult> IncrementBy(string key, long amount)
    {
        // Redis INCRBY：將 key 的值增加指定 amount
        long result = await _db.StringIncrementAsync(key, amount);
        return Ok(new { key, newValue = result });
    }
}