using StackExchange.Redis;

namespace Experiment.Services;

public class RedisQueueService(IConnectionMultiplexer redis)
{
    private readonly IDatabase _db = redis.GetDatabase();

    public async Task StartProcessAsync(string processId)
    {
        var firstProcess = ProcessRegistry.Definition.First();
        await EnqueueProcessTasksAsync(processId, firstProcess.Key);
    }

    private async Task EnqueueProcessTasksAsync(string processId, string processName)
    {
        var tasks = ProcessRegistry.Definition[processName];

        if (ProcessRegistry.IsSequential(processName))
        {
            var first = tasks.First();
            await EnqueueTaskAsync(processId, processName, first);
        }
        else
        {
            foreach (var task in tasks)
            {
                await EnqueueTaskAsync(processId, processName, task);
            }
        }
    }

    private async Task EnqueueTaskAsync(string processId, string processName, string taskName)
    {
        var queueKey = $"queue:pending:{taskName}";
        var payload = $"{processId}|{processName}|{taskName}";
        await _db.ListRightPushAsync(queueKey, payload);
    }

    public async Task HandleTaskCompletionAsync(string processId, string processName, string taskName)
    {
        var doneKey = $"process:{processId}:done:{processName}";
        await _db.SetAddAsync(doneKey, taskName);

        var allTasks = ProcessRegistry.Definition[processName];
        bool isSequential = ProcessRegistry.IsSequential(processName);

        if (isSequential)
        {
            int idx = allTasks.IndexOf(taskName);
            if (idx < allTasks.Count - 1)
            {
                var nextTask = allTasks[idx + 1];
                await EnqueueTaskAsync(processId, processName, nextTask);
                return;
            }
        }

        var doneSet = await _db.SetMembersAsync(doneKey);
        if (doneSet.Length == allTasks.Count)
        {
            await AdvanceToNextProcess(processId, processName);
        }
    }

    private async Task AdvanceToNextProcess(string processId, string currentProcess)
    {
        var processKeys = ProcessRegistry.Definition.Keys.ToList();
        var index = processKeys.IndexOf(currentProcess);

        if (index < processKeys.Count - 1)
        {
            var nextProcess = processKeys[index + 1];
            await EnqueueProcessTasksAsync(processId, nextProcess);
        }
        else
        {
            Console.WriteLine($"✅ 流程已完成：{processId}");
        }
    }
}
