namespace Experiment.Job;

[RecurringJob("*/3 * * * *")]
public class ThreeMinutelyJob : IRecurringJob
{
    public Task ExecuteAsync()
    {
        Console.WriteLine("📧 ThreeMinutelyJob triggered!");
        return Task.CompletedTask;
    }
}