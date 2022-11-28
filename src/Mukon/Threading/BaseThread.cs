using System.Globalization;

namespace Mukon.Threading;

public abstract class BaseThread : IThread
{
    private readonly Thread _thread;
    
    protected bool Running;
    protected bool Canceled => CancellationToken is {IsCancellationRequested: true};

    public required string Name { get; init; }
    public CancellationToken? CancellationToken { get; protected set; }
    public IThread.ThreadStatus Status { get; protected set; }
    public bool Joined { get; protected set; }

    Thread IThread.Thread => _thread;
    
    protected BaseThread()
    {
        Status = IThread.ThreadStatus.Paused;
        Joined = false;
        Running = false;

        _thread = new(StartThread) {Name = Name};
    }
    protected BaseThread(string name) : this() => Name = name;

    public void Start() => _thread.Start();

    public void Start(CancellationToken cancellationToken)
    {
        CancellationToken = cancellationToken;
        Start();
    }

    public void Stop()
    {
        OnStop();

        Running = false;
    }

    public bool IsAlive() =>
        _thread.IsAlive && (Running || Status switch
        {
            IThread.ThreadStatus.Running => true,
            IThread.ThreadStatus.Paused => true,
            _ => false
        });

    public void Join()
    {
        if (!Running || Joined)
        {
            return;
        }

        _thread.Join();
        Joined = true;
    }

    /// <summary>
    /// This method will be automatically called then the thread starts.
    /// </summary>
    protected virtual void StartThread()
    {
        Status = IThread.ThreadStatus.Running;
        Running = true;

        OnRun();

        Running = false;
        Status = IThread.ThreadStatus.Finished;
    }

    protected virtual void StopThread()
    {
        OnStop();
        _thread.Interrupt();

        Running = false;
        Status = IThread.ThreadStatus.Finished;
    }

    /// <summary>
    /// A thread run.
    /// </summary>
    protected abstract void OnRun();

    /// <summary>
    /// Shutdown a thread.
    /// </summary>
    protected virtual void OnStop() { }

    /// <returns>
    /// Returns if the thread is able to run.
    /// </returns>
    protected virtual bool CanRun() => !Canceled && Running;
}
