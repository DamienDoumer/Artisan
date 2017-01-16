using System;
using System.Threading;
using TimeManager.Delegates;
using TimeManager.Interfaces;

namespace TimeManager
{
    public class MeraTimer : ITimer
    {
        public event TimeManagerDelegates.TimeArrivedEventHandler TimeArrived;
        public event TimeManagerDelegates.TimerStoppedEventHandler TimeStopped;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        private bool stopVal;

        public MeraTimer(DateTime start, DateTime endTime)
        {
            stopVal = false;
            StartTime = start;
            EndTime = endTime;
        }
        
        public void Start()
        {
            stopVal = false;
            Thread thread = new Thread(new ThreadStart(CheckTimeArrived));
            thread.Start();
        }

        private void CheckTimeArrived()
        {
            while(true)
            {
                Thread.Sleep(1000);
                Console.WriteLine(EndTime.CompareTo(DateTime.Now));
                if(EndTime.CompareTo(DateTime.Now) == 1)
                {
                    if (TimeArrived != null)
                    {
                        TimeArrived(EndTime);
                    }

                    break;
                }
                if(stopVal == true)
                {
                    if(TimeStopped != null)
                    {
                        TimeStopped(DateTime.Now);
                    }

                    break;
                }
            }
        }

        public void Stop()
        {
            stopVal = true;
        }
    }
}
