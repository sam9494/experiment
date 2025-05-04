using Experiment.Services;
using Experiment.Workers;

namespace HRMWorkflow.Services;

public class FetchSalaryStructureWorker(RedisQueueService queueService, ILogger<FetchSalaryStructureWorker> logger)
    : BaseTaskWorker(queueService, logger)
{
    public override string TaskName => "fetch-salary-structure";

    protected override async Task DoWorkAsync(string processId, string processName, string taskName)
    {
        await Task.Delay(500);
    }
}