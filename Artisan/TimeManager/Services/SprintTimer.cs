using System;
using System.Threading;
using TimeManager.Delegates;
using TimeManager.Interfaces;
using TimeManager.Enum;
using Dao.Entities;
using System.Diagnostics;

namespace TimeManager
{
    /// <summary>
    /// Does all the timing calculations for any Event/WorkingSession
    /// </summary>
    public class SprintTimer : ITimer
    {
        public event TimeManagerDelegates.TimeArrivedEventHandler TimeArrived;
        public event TimeManagerDelegates.TimerStoppedEventHandler TimeStopped;
        public event TimeManagerDelegates.TimeSpanTerminatedEventHandler TimeSpanTerminated;

        /// <summary>
        /// Occures when the internal counter starts.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="time"></param>
        public delegate void CounterStartedEventHandler(WorkingSession session, DateTime time);
        public event CounterStartedEventHandler CounterStarted;
        /// <summary>
        /// Occures when the time of the counter changes.
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        public delegate void CounterTimeChangedEventHandler(int h, int m, int s, DateTime time, float percentage);
        public event CounterTimeChangedEventHandler CounterTimeChanged;
        /// <summary>
        /// Counter Ended.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="time"></param>
        public delegate void CounterEndedEventHandler(WorkingSession session, DateTime time);
        public event CounterEndedEventHandler CounterEnded;

        /// <summary>
        /// This delegate is used to refference OccuranceMonitor's CheckOccuranceTimeRange
        /// And with this, you will be able to determine wether the time span is to be changed or not right
        /// from SprintTimerObjects themselves.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public delegate TimeSelect CheckTimeRangeDelegate(DateTime date);
        public CheckTimeRangeDelegate CheckTimeRange;

        public DateTime StartTime { get { return startTime; } }
        public DateTime EndTime { get; }
        public TimeEntity Entity { get; }

        private int interval;
        private bool stopVal;
        private DateTime startTime;
        public bool isWorkingSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endTime">Time at which the timer will end</param>
        /// <param name="timeSelect">Selection range of the time which the thread should rest to wait 
        /// Before ringing</param>
        public SprintTimer(DateTime endTime, TimeSelect timeSelect)
        {
            interval = ChoseInterval(timeSelect);
            stopVal = false;
            EndTime = endTime;
        }

        /// <summary>
        /// Constructs the sprint Timer
        /// </summary>
        /// <param name="entity">The time Entity which is to be monitored, either a Working session, or an event</param>
        /// <param name="timeSelect">Selection range of the time which the thread should rest to wait 
        /// Before ringing</param>
        public SprintTimer(TimeEntity entity, TimeSelect timeSelect)
        {
            /////____________________________________________________________
            /////Debug Code
            //Debug.WriteLine("Sprint Timer created for: "+entity.GetType()+" with ID "+entity.ID+"\n");

            if (entity.GetType() == typeof(WorkingSession))
            {
                EndTime = entity.StartTime;
                isWorkingSession = true;
                TimeSpan ts = TimeSpan.FromTicks(EndTime.Ticks - entity.EndTime.Ticks);
                Entity = entity as WorkingSession;
            }
            else
            {
                interval = ChoseInterval(timeSelect);
                EndTime = entity.EndTime;
                Entity = entity as Event;
                isWorkingSession = false;
            }
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            ///____________________________________________________________
            ///Debug Code
            ///Debug.WriteLine("Timer Started for: " + Entity.GetType() + " with ID " + Entity.ID + "\n");

            stopVal = false;
            startTime = DateTime.Now;
            Thread thread = new Thread(new ThreadStart(CheckTimeArrived));
            thread.Start();
        }

        /// <summary>
        /// Checks if the time for ring has arrived
        /// </summary>
        private void CheckTimeArrived()
        {
            while (true)
            {
                Thread.Sleep(interval);

                if (EndTime.Ticks <= DateTime.Now.Ticks)
                {
                    if (TimeArrived != null)
                    {

                        /////____________________________________________________________
                        /////Debug Code
                        //Debug.WriteLine("Time arrived event fired for" 
                        //    + Entity.GetType() + " with ID " + Entity.ID + "\n");

                            TimeArrived(DateTime.Now, this);
                    }

                    break;
                }
                else
                {
                    /////____________________________________________________________
                    /////Debug Code
                    //Debug.WriteLine("End of Wating interval in "+
                    //   " CheckTimeArrived thread for: " + 
                    //   Entity.GetType() + " with ID " + Entity.ID + "\n");


                    //check the interval in which this time falls again and set
                    //a time interval in accordance to what we will get.
                    TimeSelect select = CheckTimeRange(EndTime);

                    /////____________________________________________________________
                    /////Debug Code
                    //Debug.WriteLine("After Checking if time range has changed " +
                    //   " for: " + Entity.GetType() + " with ID " + Entity.ID +" Time range = "+select+"\n");

                    interval = ChoseInterval(select);


                    /////____________________________________________________________
                    /////Debug Code
                    //Debug.WriteLine("New Interval = "+interval);

                }
                if (stopVal == true)
                {
                    if (TimeStopped != null)
                    {
                        TimeStopped(DateTime.Now, this);
                    }

                    break;
                }
            }

            ////Start Counting for the time elapsed for a working session to occure.
            //if (isWorkingSession)
            //{
            //    counter.Start();
            //}
        }
        
        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void Stop()
        {
            stopVal = true;
        }

        /// <summary>
        /// Choses the interval at which the timer will loop
        /// and check if the time has arrived
        /// </summary>
        /// <param name="timeSelect"></param>
        /// <returns></returns>
        private int ChoseInterval(TimeSelect timeSelect)
        {
            switch (timeSelect)
            {
                case TimeSelect.Second:
                    return 1000;
                case TimeSelect.Minute:
                    return 60000;
                case TimeSelect.Hour:
                    return 3600000;
                case TimeSelect.Day:
                    return 86400000;
            }
            return 0;
        }
    }
}
