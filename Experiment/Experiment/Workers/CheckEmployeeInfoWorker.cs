using Experiment.Services;
using Experiment.Workers;

namespace HRMWorkflow.Services;

public class CheckEmployeeInfoWorker(RedisQueueService queueService, ILogger<CheckEmployeeInfoWorker> logger)
    : BaseTaskWorker(queueService, logger)
{
    public override string TaskName => "check-employee-info";

    protected override async Task DoWorkAsync(string processId, string processName, string taskName)
    {
        await Task.Delay(500); // 模擬處理
    }
}