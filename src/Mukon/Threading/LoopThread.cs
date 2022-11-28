namespace Mukon.Threading;

public abstract class LoopThread : BaseThread
{
    /// <summary>
    /// Thread sleep interval in milliseconds.
    /// </summary>
    public required long ThreadInterval { get; init; }
    
    protected LoopThread() {}
    protected LoopThread(int threadInterval): this() => ThreadInterval = threadInterval;

    protected LoopThread(String name) : this() => Name = name;
    protected LoopThread(String name, int threadInterval) : this(name) => ThreadInterval = threadInterval;

    
    protected override void StartThread()
    {
        Status = IThread.ThreadStatus.Running;
        Running = true;

        long currentTimestamp = GetCurrentMillis();
        long targetTime = currentTimestamp + ThreadInterval;

        while (CanRun())
        {
            OnRun();

            long calcTime = targetTime - GetCurrentMillis();
            if (calcTime < 0)
            {
                // TODO: err / warn running behind -calcTime ms...
                
                calcTime = 0;
            }

            TimeSpan sleepTime = TimeSpan.FromMilliseconds(calcTime);
            
            Thread.Sleep(sleepTime);
            targetTime += ThreadInterval;
        }

        Running = false;
        if (Status == IThread.ThreadStatus.Running)
        {
            Status = IThread.ThreadStatus.Finished;
        }
    }

    protected static long GetCurrentMillis() => DateTimeOffset.Now.ToUnixTimeMilliseconds();
}
