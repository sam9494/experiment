using Experiment.Services;
using Experiment.Workers;

namespace HRMWorkflow.Services;

public class CalculateAdjustedSalaryWorker(
    RedisQueueService queueService,
    ILogger<CalculateAdjustedSalaryWorker> logger)
    : BaseTaskWorker(queueService, logger)
{
    public override string TaskName => "calculate-adjusted-salary";

    protected override async Task DoWorkAsync(string processId, string processName, string taskName)
    {
        await Task.Delay(500);
    }
}