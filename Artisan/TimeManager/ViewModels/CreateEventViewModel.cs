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
    public class CreateEventViewModel : BindableBase
    {
        public delegate void EventCreatedEventHandler(Event eventCreated);
        public event EventCreatedEventHandler EventCreated;

        private DateTime date;
        private EventDao eventDao;
        public RelayCommand CreateCommand { get; private set; }
        public string Name { get; set; }
        public string Venue { get; set; }
        public string Description { get; set; }
        public DateTime Date { get { return date; } set { date = value; } }
        public DateTime Time { get; set; }

        public CreateEventViewModel()
        {
            CreateCommand = new RelayCommand(OnCreateEvent);
            eventDao = new EventDao("Event");
            Date = DateTime.Now;
        }

        private void OnCreateEvent()
        {
            ///Create the exact day and time at which the event should be fired.
            DateTime finalDate = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, Time.Second);
            Event newEvent = new Event(Name, Description, Venue, finalDate);
            eventDao.Save(newEvent);
            EventCreated?.Invoke(newEvent);
        }
    }
}
