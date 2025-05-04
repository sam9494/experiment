namespace Experiment.Services;

public class QueueWorker(RedisQueueService queueService, ILogger<QueueWorker> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var task = await queueService.BlockingPopAndPushAsync("queue:pending", "queue:processing", 5);
            if (task != null)
            {
                logger.LogInformation("Processing task: {task}", task);
                try
                {
                    // 模擬失敗處理邏輯（你可以改成真實邏輯）
                    if (task.Contains("fail"))
                        throw new Exception("模擬錯誤");

                    await Task.Delay(100000); // 模擬處理任務
                    await queueService.RemoveFromProcessingAsync("queue:processing", task);
                    logger.LogInformation("Task completed: {task}", task);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing task: {task}", task);
                    var retryCount = await queueService.IncrementRetryCountAsync(task);
                    if (retryCount >= 3)
                    {
                        await queueService.RemoveFromProcessingAsync("queue:processing", task);
                        await queueService.MoveToDeadLetterAsync(task);
                        logger.LogWarning("Moved to dead-letter after {retryCount} attempts: {task}", retryCount, task);
                    }
                    else
                    {
                        logger.LogWarning("Retry {retryCount} for task: {task}", retryCount, task);
                    }
                }
            }
        }
    }
}