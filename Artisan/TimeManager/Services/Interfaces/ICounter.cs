using TimeManager.Delegates;

namespace TimeManager.Interfaces
{
    public interface ICounter
    {
        event TimeManagerDelegates.CounterEndedEventHandler CountEnded;
        event TimeManagerDelegates.CounterTimeChanged TimeChanged;
        event TimeManagerDelegates.CounterStartedEventHandler CountStarted;

        void Start();
        void Stop();
        void Pause();
    }
}
