using Experiment.Services;
using Experiment.Workers;

namespace HRMWorkflow.Services;

public class CreateApprovalRequestWorker(RedisQueueService queueService, ILogger<CreateApprovalRequestWorker> logger)
    : BaseTaskWorker(queueService, logger)
{
    public override string TaskName => "create-approval-request";

    protected override async Task DoWorkAsync(string processId, string processName, string taskName)
    {
        await Task.Delay(500);
    }
}