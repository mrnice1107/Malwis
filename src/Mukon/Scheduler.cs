using System.Collections.Concurrent;
using Mukon.Threading;

namespace Mukon;

public class Scheduler
{
    #region Static
    static Scheduler() => Instance = new();

    public static Scheduler Instance { get; }
    #endregion

    private readonly IList<IThread> _runningThreads;
    private readonly ConcurrentBag<ThreadSchedule> _scheduledThreads;

    private Scheduler()
    {
        _runningThreads = new List<IThread>();
        _scheduledThreads = new();
    }

    public void ScheduleThread(IThread scheduledThread, DateTime startTime, DateTime? endTime = null) => ScheduleThread(new(scheduledThread, startTime, endTime));
    public void ScheduleThread(ThreadSchedule schedule) => _scheduledThreads.Add(schedule);
}

public record struct ThreadSchedule(IThread ScheduledThread, DateTime StartTime, DateTime? EndTime = null);

