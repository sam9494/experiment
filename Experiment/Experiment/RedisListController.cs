using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Experiment;

[ApiController]
[Route("[controller]")]
public class RedisListController : ControllerBase
{
    private readonly IDatabase _db;

    public RedisListController(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    [HttpPost("rpush")]
    public async Task<IActionResult> RPush(string key, string value)
    {
        long len = await _db.ListRightPushAsync(key, value);
        return Ok(new { key, value, newLength = len });
    }

    [HttpPost("lpush")]
    public async Task<IActionResult> LPush(string key, string value)
    {
        long len = await _db.ListLeftPushAsync(key, value);
        return Ok(new { key, value, newLength = len });
    }


    [HttpPost("lpop")]
    public async Task<IActionResult> LPop(string key)
    {
        var value = await _db.ListLeftPopAsync(key);
        return Ok(new { key, popped = value.ToString() });
    }

    [HttpPost("rpop")]
    public async Task<IActionResult> RPop(string key)
    {
        var value = await _db.ListRightPopAsync(key);
        return Ok(new { key, popped = value.ToString() });
    }
    [HttpGet("lindex")]
    public async Task<IActionResult> LIndex(string key, int index)
    {
        var value = await _db.ListGetByIndexAsync(key, index);
        return Ok(new { key, index, value = value.ToString() });
    }
    
    [HttpGet("lrange")]
    public async Task<IActionResult> LRange(string key, int start = 0, int stop = -1)
    {
        var values = await _db.ListRangeAsync(key, start, stop);
        return Ok(values.Select(v => v.ToString()));
    }

    [HttpPost("ltrim")]
    public async Task<IActionResult> LTrim(string key, int start, int stop)
    {
        await _db.ListTrimAsync(key, start, stop);
        return Ok(new { key, trimmedRange = $"{start}-{stop}" });
    }


}