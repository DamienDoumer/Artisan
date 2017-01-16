using System;

namespace TimeManager.Delegates
{
    public class TimeManagerDelegates
    {
        public delegate void TimeArrivedEventHandler(DateTime endTime, SprintTimer timer);
        public delegate void TimerStoppedEventHandler(DateTime stop, SprintTimer timer);
        public delegate void TimeSpanTerminatedEventHandler(DateTime endTime, SprintTimer timer);

        public delegate void CounterEndedEventHandler(DateTime endTime, object obj);
        public delegate void CounterTimeChanged(int hour, int min, int sec,
            DateTime timeOfChange, float percentage);
        public delegate void CounterStartedEventHandler(DateTime date, object obj);
    }
}
