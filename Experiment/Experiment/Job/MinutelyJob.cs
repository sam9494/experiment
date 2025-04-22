namespace Experiment.Job;

[RecurringJob("* * * * *")]
public class MinutelyJob : IRecurringJob
{
    public Task ExecuteAsync()
    {
        Console.WriteLine("📧 MinutelyJob triggered!");
        return Task.CompletedTask;
    }
}