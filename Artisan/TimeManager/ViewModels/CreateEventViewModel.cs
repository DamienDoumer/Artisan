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
        public DateTime Date { get { return date; } set { date = value; } }
        public DateTime Time { get; set; }
        static public string Title { get; set; }
        /// <summary>
        /// This is the event bound to the view
        /// </summary>
        public static Event MainEvent
        { get { return mainEvent; } set { mainEvent = value; oldEvent = value; } }

        public CreateEventViewModel()
        {
            mainEvent = new Event();
            mainEvent.Date_Time = DateTime.Now;
            CreateCommand = new RelayCommand(OnCreateEvent);
            eventDao = new EventDao("Event");
            Date = DateTime.Now;
        }

        private void OnCreateEvent()
        {
            Debug.WriteLine(MainEvent == null);
            Debug.WriteLine(MainEvent.Date_Time == null);
            ///Create the exact day and time at which the event should be fired.
            DateTime finalDate = new DateTime(mainEvent.Date_Time.Year, mainEvent.Date_Time.Month,
                mainEvent.Date_Time.Day, mainEvent.Date_Time.Hour,
                mainEvent.Date_Time.Minute, mainEvent.Date_Time.Second);

            mainEvent.Date_Time = finalDate;
            
            if(Title == "Edit Event") { eventDao.UpdateEvent(oldEvent, mainEvent); }
            else
            {
                eventDao.Save(mainEvent);
            }
            EventCreated?.Invoke(mainEvent);
        }
    }
}
