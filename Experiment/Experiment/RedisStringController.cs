using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Experiment;

[ApiController]
[Route("[controller]")]
public class RedisStringController : ControllerBase
{
    private readonly IDatabase _db;

    public RedisStringController(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }
    [HttpPost("append")]
    public async Task<IActionResult> AppendValue(string key, string value)
    {
        // Redis APPEND：將字串附加到 key 對應的字串後面，回傳結果為新字串的長度
        long newLength = await _db.StringAppendAsync(key, value);
        return Ok(new { key, appended = value, newLength });
    }
    
    [HttpPost("setbit")]
    public async Task<IActionResult> SetBit(string key, long offset, bool bit)
    {
        // Redis SETBIT：設定指定 offset 的 bit 為 0 或 1
        bool previousBit = await _db.StringSetBitAsync(key, offset, bit);
        return Ok(new { key, offset, newBit = bit, previousBit });
    }
   
}