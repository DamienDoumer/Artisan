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

        private static Event oldEvent;
        static Event mainEvent;
        private DateTime date;
        private EventDao eventDao;
        public RelayCommand CreateCommand { get; private set; }
        public DateTime Date { get { return date; } set { SetProperty(ref date, value); } }
        public DateTime Time { get; set; }
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
            ///Create the exact day and time at which the event should be fired.
            DateTime finalDate = new DateTime(Date.Year, Date.Month,
               Date.Day, Time.Hour,
                Time.Minute, Time.Second);

            ///setting these two differently is very important
            mainEvent.Date_Time = finalDate;
            mainEvent.EndTime = finalDate;
            
            if(Title == "Edit Event") { eventDao.UpdateEvent(oldEvent, mainEvent); }
            else
            {
                eventDao.Save(mainEvent);
            }

            OccuranceMonitor.Instance.StartMonitoring(mainEvent);

            EventCreated?.Invoke(mainEvent);
        }
    }
}
