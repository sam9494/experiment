using Experiment.Services;
using Experiment.Workers;

namespace HRMWorkflow.Services;

public class CheckChangeTypeWorker(RedisQueueService queueService, ILogger<CheckChangeTypeWorker> logger)
    : BaseTaskWorker(queueService, logger)
{
    public override string TaskName => "check-change-type";

    protected override async Task DoWorkAsync(string processId, string processName, string taskName)
    {
        await Task.Delay(500);
    }
}