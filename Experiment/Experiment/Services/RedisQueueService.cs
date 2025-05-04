using StackExchange.Redis;

namespace Experiment.Services
{
    public class RedisQueueService(IConnectionMultiplexer redis)
    {
        public async Task EnqueueAsync(string queueKey, string value)
        {
            await redis.GetDatabase().ListRightPushAsync(queueKey, value);
        }

        public async Task<string?> BlockingPopAndPushAsync(string source, string destination, int timeoutSeconds)
        {
            var result = await redis.GetDatabase().ExecuteAsync("BRPOPLPUSH", source, destination, timeoutSeconds);
            return result.IsNull ? null : result.ToString();
        }

        public async Task RemoveFromProcessingAsync(string processingKey, string value)
        {
            await redis.GetDatabase().ListRemoveAsync(processingKey, value);
        }

        public async Task<long> IncrementRetryCountAsync(string taskKey)
        {
            var db = redis.GetDatabase();
            var retryKey = $"retry:{taskKey}";
            var count = await db.StringIncrementAsync(retryKey);
            await db.KeyExpireAsync(retryKey, TimeSpan.FromHours(1));
            return count;
        }

        public async Task MoveToDeadLetterAsync(string value)
        {
            await redis.GetDatabase().ListRightPushAsync("queue:deadletter", value);
        }
    }
}