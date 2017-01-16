using System;
using TimeManager.Delegates;
using TimeManager.Interfaces;
using System.Threading;
using System.Diagnostics;
using Dao.Entities;

namespace TimeManager
{
    public class DownCounter : ICounter
    {
        public int Time { get; }
        public TimeEntity Entity { get; set; }


        private int seconds;
        private int minutes;
        private int hours;
        private int interval;
        private bool stopVal;
        //private bool pauseVal;

        public event TimeManagerDelegates.CounterEndedEventHandler CountEnded;
        public event TimeManagerDelegates.CounterTimeChanged TimeChanged;
        public event TimeManagerDelegates.CounterStartedEventHandler CountStarted;

        public DownCounter(int hour, int min, int sec, TimeEntity entity)
        {
            //This is to change the time into the total number of seconds left to ring.
            hours = hour;
            minutes = min;
            seconds = sec;
            interval = 1000;
            Entity = entity;

            int time = hours * 3600;
            time = time + (minutes * 60);
            time = time + seconds;
            Time = time;
            stopVal = false;
        }

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(
                () => 
                {
                    float percentage = 0.0F;

                    if (CountStarted != null) CountStarted(DateTime.Now, Entity);

                    for (int i = 0; i <= Time; i++)
                    {
                        percentage = (i * 100) / Time;

                        if (stopVal != true)
                        {
                            Thread.Sleep(interval);
                            
                            if (TimeChanged != null)
                            {
                                TimeChanged(hours,
                                        minutes, seconds, DateTime.Now, percentage);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    CountEnded?.Invoke(DateTime.Now, Entity);
                }
                ));

            thread.Start();
        }

        public void Stop()
        {
            stopVal = true;
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }
    }
}
