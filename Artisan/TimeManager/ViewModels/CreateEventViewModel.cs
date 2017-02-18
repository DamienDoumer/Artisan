using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artisan.MVVMShared;
using Dao.Entities;
using Dao;
using System.Diagnostics;

namespace TimeManager.ViewModels
{
    /// <summary>
    /// This ViewModel and its corresponding View are used directly to 
    /// Create and Edit Events, this is done by changing Certain main properties
    /// </summary>
    public class CreateEventViewModel : BindableBase
    {
        public static event Action<Event> EventCreated;
        public static event Action<string> NotificationNeeded;

        private static Event oldEvent;
        static Event mainEvent;
        private DateTime date;
        private DateTime time;
        private EventDao eventDao;
        public RelayCommand CreateCommand { get; private set; }
        public DateTime Date
        {
            get { return date; }
            set
            {
                SetProperty(ref date, value);
                Time = new DateTime(Date.Year, Date.Month, Date.Day);
            }
        }
        public DateTime Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }
        static public string Title { get; set; }
        /// <summary>
        /// This is the event bound to the view
        /// </summary>
        public static Event MainEvent
        { get { return mainEvent; } set { mainEvent = value; oldEvent = value; } }

        public CreateEventViewModel()
        {
            if (Title == "Create Event")
            {
                mainEvent = new Event();
                Date = DateTime.Now;
            }
            else
            if(Title == "Edit Event")
            {
                Date = mainEvent.Date_Time;
                Time = mainEvent.Date_Time;
            }

            CreateCommand = new RelayCommand(OnCreateEvent);
            eventDao = new EventDao("Event");
        }

        private void OnCreateEvent()
        {
            StringBuilder errorMessages = new StringBuilder();
            bool error = false;
            //this is used to make the second error message apear once only
            bool eventProperror = false;
          
            ///Create the exact day and time at which the event should be fired.
            DateTime finalDate = new DateTime(Date.Year, Date.Month,
               Date.Day, Time.Hour,
                Time.Minute, Time.Second);

            if (finalDate.Ticks <= DateTime.Now.Ticks)
            {
                errorMessages.Append("You cannot create an event for a date which has passed.\n");
                error = true;
            }
            if(MainEvent.Name == "")
            {
                errorMessages.Append("You must enter the event's title, venue and description.\n");
                error = true;
                eventProperror = true;
            }
            if(MainEvent.Venue == "" && !eventProperror)
            {
                errorMessages.Append("You must enter the event's title, venue and description.\n");
                error = true;
                eventProperror = true;
            }
            if(MainEvent.Description == "" && !eventProperror)
            {
                errorMessages.Append("You must enter the event's title, venue and description.\n");
                error = true;
                eventProperror = true;
            }

            if (!error)
            {
                ///setting these two differently is very important
                mainEvent.Date_Time = finalDate;
                mainEvent.EndTime = finalDate;

                if (Title == "Edit Event") { eventDao.UpdateEvent(oldEvent, mainEvent); }
                else
                {
                    eventDao.Save(mainEvent);
                }

                OccuranceMonitor.Instance.StartMonitoring(mainEvent);

                EventCreated?.Invoke(mainEvent);
            }
            else
            {
                ///Notify the outer layer for an error
                NotificationNeeded?.Invoke(errorMessages.ToString());
                error = false;
                eventProperror = false;
            }
        }
    }
}
