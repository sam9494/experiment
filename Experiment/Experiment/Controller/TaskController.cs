using Experiment.Services;
using Microsoft.AspNetCore.Mvc;

namespace Experiment.Controller;

[ApiController]
[Route("[controller]")]
public class TaskController(RedisQueueService queueService) : ControllerBase
{
    /// <summary>
    /// 模擬 worker 回報任務完成
    /// </summary>
    [HttpPost("complete")]
    public async Task<IActionResult> CompleteTask(
        string processId,
        string processName,
        string taskName)
    {
        await queueService.HandleTaskCompletionAsync(processId, processName, taskName);
        return Ok($"✅ {taskName} 已標記完成。");
    }

    /// <summary>
    /// 手動啟動流程（第一個流程的任務會進入 queue）
    /// </summary>
    [HttpPost("start")]
    public async Task<IActionResult> Start(string processId)
    {
        await queueService.StartProcessAsync(processId);
        return Ok($"🚀 流程 {processId} 已啟動！");
    }
}