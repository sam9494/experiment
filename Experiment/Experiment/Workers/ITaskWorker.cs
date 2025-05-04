namespace Experiment.Services;

/// <summary>
/// 所有任務 Worker 都應該實作這個介面
/// </summary>
public interface ITaskWorker
{
    /// <summary>
    /// 該 Worker 對應的任務名稱（ex: check-employee-info）
    /// </summary>
    string TaskName { get; }

    /// <summary>
    /// 實際執行任務的邏輯
    /// </summary>
    Task ExecuteAsync(string processId, string processName, string taskName);
}