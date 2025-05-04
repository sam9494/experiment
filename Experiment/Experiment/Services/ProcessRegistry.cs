namespace Experiment.Services;

public static class ProcessRegistry
{
    /// <summary>
    /// 定義所有流程與對應任務（順序 = 任務順序，並行 = 任務順序無關）
    /// </summary>
    public static readonly Dictionary<string, List<string>> Definition = new()
    {
        ["validate-employee-change"] = new List<string>
        {
            "check-employee-info",
            "check-change-type"
        },
        ["calculate-salary-adjustment"] = new List<string>
        {
            "fetch-salary-structure",
            "calculate-adjusted-salary"
        },
        ["initiate-approval-flow"] = new List<string>
        {
            "create-approval-request",
            "notify-approver"
        }
    };

    /// <summary>
    /// 哪些流程需要任務「順序執行」
    /// </summary>
    private static readonly HashSet<string> SequentialProcesses = new()
    {
        "calculate-salary-adjustment",
        "initiate-approval-flow"
    };

    public static bool IsSequential(string processName)
    {
        return SequentialProcesses.Contains(processName);
    }
}