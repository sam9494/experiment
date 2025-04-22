namespace Experiment;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RecurringJobAttribute : Attribute
{
    public string Cron { get; }

    public RecurringJobAttribute(string cron)
    {
        Cron = cron;
    }
}