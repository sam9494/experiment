namespace Experiment.Job;

public interface IRecurringJob
{
    Task ExecuteAsync();
}