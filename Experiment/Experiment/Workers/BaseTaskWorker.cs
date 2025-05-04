using Experiment.Services;

namespace Experiment.Workers;

public abstract class BaseTaskWorker : ITaskWorker
{
    private readonly RedisQueueService _queueService;
    private readonly ILogger _logger;

    public abstract string TaskName { get; }

    protected BaseTaskWorker(RedisQueueService queueService, ILogger logger)
    {
        _queueService = queueService;
        _logger = logger;
    }

    public async Task ExecuteAsync(string processId, string processName, string taskName)
    {
        _logger.LogInformation("執行任務 {taskName}", taskName);
        await DoWorkAsync(processId, processName, taskName);
        await _queueService.HandleTaskCompletionAsync(processId, processName, taskName);
        _logger.LogInformation("完成任務 {taskName}", taskName);
    }

    protected abstract Task DoWorkAsync(string processId, string processName, string taskName);
}