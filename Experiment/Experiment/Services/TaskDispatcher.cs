using Experiment.Services;
using StackExchange.Redis;

namespace HRMWorkflow.Services;

public class TaskDispatcher : BackgroundService
{
    private readonly IDatabase _db;
    private readonly Dictionary<string, ITaskWorker> _workerMap;
    private readonly ILogger<TaskDispatcher> _logger;

    public TaskDispatcher(
        IConnectionMultiplexer redis,
        IEnumerable<ITaskWorker> workers,
        ILogger<TaskDispatcher> logger)
    {
        _db = redis.GetDatabase();
        _logger = logger;
        _workerMap = workers.ToDictionary(w => w.TaskName);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🚦 Task Dispatcher 啟動");

        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var taskName in _workerMap.Keys)
            {
                string queueKey = $"queue:pending:{taskName}";
                var result = await _db.ListLeftPopAsync(queueKey);

                if (!result.IsNullOrEmpty)
                {
                    var parts = result.ToString().Split('|');
                    if (parts.Length != 3) continue;

                    string processId = parts[0];
                    string processName = parts[1];
                    string actualTask = parts[2];

                    if (_workerMap.TryGetValue(actualTask, out var worker))
                    {
                        _logger.LogInformation("🎯 派發任務 {task} → Worker（流程 {process}, ID {id}）", actualTask, processName, processId);
                        await worker.ExecuteAsync(processId, processName, actualTask);
                    }
                    else
                    {
                        _logger.LogWarning("⚠️ 找不到對應的 Worker：{task}", actualTask);
                    }
                }
            }

            await Task.Delay(500);
        }
    }
}