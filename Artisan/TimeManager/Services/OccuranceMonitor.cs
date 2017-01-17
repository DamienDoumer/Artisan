using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TimeManager.Enum;
using Dao.Entities;
using Dao;
using TimeManager.Interfaces;

namespace TimeManager
{
    /// <summary>
    /// Control's Loading, Sorting, Occurances and notifies the following application layer for any 
    /// Time Arrived event, Time Change Event, and any other information needed by the User 
    /// </summary>
    public class OccuranceMonitor
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static OccuranceMonitor instance;
        public static OccuranceMonitor Instance
        {
            get { return instance; } set { instance = value; }
        }

        private List<TimeEntity> pastEntities;
        private List<TimeEntity> futureEntities;
        public List<Event> PresentEvents { get; }

        /// <summary>
        /// Fired when the time for an alarm has arrived.
        /// </summary>
        /// <param name="entity"></param>
        public delegate void AlarmTimeArrivedEventHandler(TimeEntity entity);
        public delegate void AlarmTimeStoppedEventHandler(TimeEntity entity);
        public event AlarmTimeArrivedEventHandler AlarmTimeArrived;
        public event AlarmTimeArrivedEventHandler AlarmTimeStopped;
        /// <summary>
        /// These events are to handle the
        /// internal events and push them to the UI or another layer of the application
        /// </summary>
        /// <param name="session"></param>
        /// <param name="time"></param>
        public delegate void CounterStartedEventHandler(WorkingSession session, DateTime time);
        public event CounterStartedEventHandler CounterStarted;
        public delegate void CounterTimeChangedEventHandler(int h, int m, int s, DateTime time, float percentage);
        public event CounterTimeChangedEventHandler CounterTimeChanged;
        public delegate void CounterEndedEventHandler(DateTime time, WorkingSession wrk);
        public event CounterEndedEventHandler CounterEnded;

        public List<TimeEntity> PastEntities
        {
            get { return pastEntities; }
            set { pastEntities = value; }
        }
        public List<TimeEntity> FutureEntities
        {
            get { return futureEntities; }
            set { futureEntities = value; }
        }

        WorkingSessionDao workingSessionDao;
        EventDao eventDao;

        /// <summary>
        /// Singleton constructors
        /// </summary>
        private OccuranceMonitor()
        {
            pastEntities = new List<TimeEntity>();
            futureEntities = new List<TimeEntity>();
            PresentEvents = new List<Event>();
            workingSessionDao = new WorkingSessionDao("WorkingSession");
            eventDao = new EventDao("Event");
        }
        static OccuranceMonitor()
        {
            OccuranceMonitor.instance = new OccuranceMonitor();
        }

        public void SortTimeEntities()
        {
            /////____________________________________________________________
            /////Debug Code
            //Debug.WriteLine("\nLOADING Time entities...\n");

            foreach (WorkingSession wrk in workingSessionDao.RetrieveAllWorkingSessions())
            {
                if (HasPassed(wrk.EndTime))
                {
                    pastEntities.Add(wrk);

                    /////____________________________________________________________
                    /////Debug Code
                    //Debug.WriteLine(" PAST " + wrk.ID + "Workingsession Was loaded from the database...");

                }
                else
                {
                    futureEntities.Add(wrk);

                    /////____________________________________________________________
                    /////Debug Code
                    //Debug.WriteLine(" FUTURE" + wrk.ID + " Workingsession Was loaded from the database...\n");

                }

            }
            foreach (Event evt in eventDao.RetrieveAllEvents())
            {
                if (HasPassed(evt.EndTime))
                {
                    pastEntities.Add(evt);

                    /////____________________________________________________________
                    /////Debug Code
                    //Debug.WriteLine(" PAST Event"+ evt.ID + "Was loaded from the database...");
                }
                else
                {
                    futureEntities.Add(evt);

                    /////____________________________________________________________
                    /////Debug Code
                    //Debug.WriteLine(" FUTURE " + evt.ID + "Event Was loaded from the database...");
                }

                if(evt.Date_Time.DayOfYear == DateTime.Now.DayOfYear)
                {
                    PresentEvents.Add(evt);

                    /////____________________________________________________________
                    /////Debug Code
                    //Debug.WriteLine(" PRESENT" + evt.ID + " Event Was loaded from the database...");
                }
            }
        }
        /// <summary>
        /// this is used to sort events which are given in and 
        /// Send them out when needed.
        /// </summary>
        /// <param name="evts"></param>
        /// <param name="present"></param>
        /// <param name="future"></param>
        /// <param name="past"></param>
        public void SortEvents(out List<Event> present, out List<Event> future, out List<Event> past)
        {
            past = new List<Event>();
            future = new List<Event>();
            present = new List<Event>();

            foreach (Event evt in eventDao.RetrieveAllEvents())
            {
                if (HasPassed(evt.EndTime))
                {
                    past.Add(evt);

                    /////____________________________________________________________
                    /////Debug Code
                    //Debug.WriteLine(" PAST Event"+ evt.ID + "Was loaded from the database...");
                }
                else
                {
                    future.Add(evt);

                    /////____________________________________________________________
                    /////Debug Code
                    //Debug.WriteLine(" FUTURE " + evt.ID + "Event Was loaded from the database...");
                }

                if (evt.Date_Time.DayOfYear == DateTime.Now.DayOfYear)
                {
                    present.Add(evt);

                    /////____________________________________________________________
                    /////Debug Code
                    //Debug.WriteLine(" PRESENT" + evt.ID + " Event Was loaded from the database...");
                }
            }
        }
        public void StartMonitoring()
        {
            SprintTimer timer = null;

            /////____________________________________________________________
            /////Debug Code
            //Debug.WriteLine("\nMONITORING TimeEntities Started\n");

            foreach (TimeEntity ent in futureEntities)
            {
                /////____________________________________________________________
                /////Debug Code
                //Debug.WriteLine(" In Time Range Selection...");

                TimeSelect selection = CheckOccuranceTimeRange(ent.EndTime);

                /////____________________________________________________________
                /////Debug Code
                //Debug.WriteLine("This "+ent.GetType()+" with ID = "+ent.ID+" Has TimeRange OF: "+selection);

                if (selection == TimeSelect.Month)
                {
                    continue;
                }
                else
                    if (selection == TimeSelect.Day)
                {
                    continue;
                }
                else
                    if (selection == TimeSelect.Hour)
                {
                    timer = new SprintTimer(ent, TimeSelect.Hour);
                }
                else
                    if (selection == TimeSelect.Minute)
                {
                    timer = new SprintTimer(ent, TimeSelect.Minute);
                }
                else
                if (selection == TimeSelect.Second)
                {
                    timer = new SprintTimer(ent, TimeSelect.Second);
                }

                /////____________________________________________________________
                /////Debug Code
                //Debug.WriteLine("Selection finished for This " + ent.GetType() + " with ID = " + ent.ID);

                timer.CheckTimeRange = new SprintTimer.CheckTimeRangeDelegate(CheckOccuranceTimeRange);
                timer.TimeArrived += OnTimeArrived;
                timer.TimeStopped += OnTimeStopped;

                timer.Start();
            }
        }
        /// <summary>
        /// Start monitoring time for only one entity.
        /// </summary>
        /// <param name="ent"></param>
        public void StartMonitoring(TimeEntity ent)
        {
            SprintTimer timer = null;

            TimeSelect selection = CheckOccuranceTimeRange(ent.EndTime);

            if (selection == TimeSelect.Hour)
            {
                timer = new SprintTimer(ent, TimeSelect.Hour);
            }
            else
                    if (selection == TimeSelect.Minute)
            {
                timer = new SprintTimer(ent, TimeSelect.Minute);
            }
            else
                if (selection == TimeSelect.Second)
            {
                timer = new SprintTimer(ent, TimeSelect.Second);
            }

            timer.CheckTimeRange = new SprintTimer.CheckTimeRangeDelegate(CheckOccuranceTimeRange);
            timer.TimeArrived += OnTimeArrived;
            timer.TimeStopped += OnTimeStopped;

            timer.Start();
        }

        private void OnTimeArrived(DateTime end, SprintTimer timer)
        {
            AlarmTimeArrived?.Invoke(timer.Entity);

            if(timer.isWorkingSession)
            {
                WorkingSession session = timer.Entity as WorkingSession;
                long time = timer.Entity.EndTime.Ticks - timer.Entity.StartTime.Ticks;
                TimeSpan span = new TimeSpan(time);

                DownCounter counter = new DownCounter(span.Hours,
                    span.Minutes, span.Seconds, session);
                counter.CountStarted += OnCounterStarted;
                counter.CountEnded += OnCounterStopped;
                counter.TimeChanged += OnCounterTimeChanged;
                counter.Start();

                /////____________________________________________________________
                /////Debug Code
                //Debug.WriteLine("Inside Time Arrived for :" + session.ID + "\n");
            }
        }
        private void OnTimeStopped(DateTime end, SprintTimer timer)
        {
                AlarmTimeStopped?.Invoke(timer.Entity);
        }

        private void OnCounterStarted(DateTime time, object wrk)
        {
            WorkingSession ses = wrk as WorkingSession;

            /////____________________________________________________________
            /////Debug Code
            //Debug.WriteLine("Counter started for working session :" + ses.ID+"\n");

            CounterStarted?.Invoke(ses, time);
        }
        private void OnCounterTimeChanged(int h, int m, int s, DateTime time, float percentage)
        {
            CounterTimeChanged?.Invoke(h, m, s, time, percentage);

            /////____________________________________________________________
            /////Debug Code
            //Debug.WriteLine("Counter Time Change for working session :"+ "\n");
        }
        private void OnCounterStopped(DateTime time, object obj)
        {
            WorkingSession wrk = obj as WorkingSession;
            //Save this working session as done working session.
            workingSessionDao.SaveAsDoneWorkingSession(wrk);

           CounterEnded?.Invoke(time, wrk);
        }

        /// <summary>
        /// Detects the occurance time range
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public TimeSelect CheckOccuranceTimeRange(DateTime date)
        {
            if (IsMoreThanADayToOccure(date))
            {
                return TimeSelect.Day;
            }
            else
            if (IsMoreThanAnHourToOccure(date))
            {
                return TimeSelect.Hour;
            }
            else
                if (IsMoreThanAMinutesToOccure(date))
                return TimeSelect.Minute;
            else
                if (IsMoreThanASecondToOccure(date))
                return TimeSelect.Second;

            return TimeSelect.Month;
        }//Works

        /// <summary>
        /// Checks if the date has passed or not.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private bool HasPassed(DateTime date)
        {
            if (date.Ticks > DateTime.Now.Ticks)
            {
                return false;
            }
            return true;
        }//Works
        private bool IsMoreThanADayToOccure(DateTime date)
        {
            long interval = date.Ticks - DateTime.Now.Ticks;

            if (interval > 864000000000 && interval <= 25920000000000)
            {
                return true;
            }
            return false;
        }//Works
        private bool IsMoreThanAnHourToOccure(DateTime date)
        {
            long interval = date.Ticks - DateTime.Now.Ticks;
            if (interval > 36000000000 && interval <= 864000000000)
            {
                return true;
            }
            return false;
        }//Works
        private bool IsMoreThanAMinutesToOccure(DateTime date)
        {
            long interval = date.Ticks - DateTime.Now.Ticks;
            if (interval >= 600000000 && interval <= 36000000000)
            {
                return true;
            }
            return false;
        }//Works
        private bool IsMoreThanASecondToOccure(DateTime date)
        {
            long interval = date.Ticks - DateTime.Now.Ticks;
            if (interval < 600000000 && interval > 10)
            {
                return true;
            }
            return false;
        }//Works
    }
}
