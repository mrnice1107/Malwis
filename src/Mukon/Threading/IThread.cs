namespace Mukon.Threading;

public interface IThread
{
    internal Thread Thread { get; }
    
    /// <summary>
    /// The name of the thread.
    /// </summary>
    public String Name { get; }
    
    /// <summary>
    /// The <see cref="CancellationToken"/> of the thread.<br/>
    /// This may be null if the thread was not started yet or if no token was given.
    /// </summary>
    public CancellationToken? CancellationToken { get; }
    
    /// <summary>
    /// The current status of the thread.
    /// </summary>
    public ThreadStatus Status { get; }

    /// <summary>
    /// If the thread is joined with the main thread.
    /// </summary>
    public bool Joined { get; }
    
    /// <summary>
    /// Starts the thread.
    /// </summary>
    public void Start();
    /// <summary>
    /// Starts the thread.
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> can be used to cancel/stop the thread.</param>
    public void Start(CancellationToken cancellationToken);

    /// <summary>
    /// Stops the thread.<br/>
    /// </summary>
    public void Stop();

    /// <returns> Weather the thread is running or not.</returns>
    public bool IsAlive();

    /// <summary>
    /// Joins the thread to the main thread.
    /// </summary>
    public void Join();

    
    /// <summary>
    /// This enum represents the current status of a thread.
    /// </summary>
    public enum ThreadStatus
    {
        /// <summary>
        /// The thread is running.
        /// </summary>
        Running,
        /// <summary>
        /// The thread finished.
        /// </summary>
        Finished,
        /// <summary>
        /// Thread is paused, not started or idle.<br/>
        /// This can happen when the thread is not started yet or is idle waiting for something.<br/>
        /// This could for example be a loop thread waiting for it's interval.
        /// </summary>
        Paused,
        /// <summary>
        /// The thread ran into some kind of error or failed.
        /// </summary>
        Failed
    }

}
