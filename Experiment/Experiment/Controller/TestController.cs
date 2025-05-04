using Microsoft.AspNetCore.Mvc;

namespace Experiment;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{

    public TestController()
    {
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        return Ok("Hello");
    }
    
}