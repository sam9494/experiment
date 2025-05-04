using Experiment.Services;
using Experiment.Workers;

namespace HRMWorkflow.Services;

public class NotifyApproverWorker(RedisQueueService queueService, ILogger<NotifyApproverWorker> logger)
    : BaseTaskWorker(queueService, logger)
{
    public override string TaskName => "notify-approver";

    protected override async Task DoWorkAsync(string processId, string processName, string taskName)
    {
        await Task.Delay(500);
    }
}