using TimeManager.Delegates;

namespace TimeManager.Interfaces
{
    interface ITimer
    {
        event TimeManagerDelegates.TimeArrivedEventHandler TimeArrived;
        event TimeManagerDelegates.TimerStoppedEventHandler TimeStopped;
        event TimeManagerDelegates.TimeSpanTerminatedEventHandler TimeSpanTerminated;

        void Start();
        void Stop();
    }
}
