using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Experiment;

[ApiController]
[Route("[controller]")]
public class RedisGetController : ControllerBase
{
    private readonly IDatabase _db;

    public RedisGetController(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetValue(string key)
    {
        var value = await _db.StringGetAsync(key);
        return Ok(new { key, value = value.ToString() });
    }
    
    [HttpGet("getbit")]
    public async Task<IActionResult> GetBit(string key, long offset)
    {
        bool bit = await _db.StringGetBitAsync(key, offset);
        return Ok(new { key, offset, bit });
    }
    
    [HttpGet("bitcount")]
    public async Task<IActionResult> GetBitCount(string key)
    {
        long count = await _db.StringBitCountAsync(key);
        return Ok(new { key, bitCount = count });
    }
    
    [HttpGet("bitdump")]
    public async Task<IActionResult> DumpAllBits(string key, int maxOffset = 10000)
    {
        var result = new Dictionary<int, bool>();

        for (int i = 0; i < maxOffset; i++)
        {
            bool bit = await _db.StringGetBitAsync(key, i);
            if (bit) // 你也可以選擇只回傳 true 的 bit
                result[i] = bit;
        }

        return Ok(result);
    }


}