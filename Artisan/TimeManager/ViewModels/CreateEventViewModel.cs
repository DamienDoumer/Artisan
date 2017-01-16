using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artisan.MVVMShared;
using Dao.Entities;
using Dao;

namespace TimeManager.ViewModels
{
    public class CreateEventViewModel : BindableBase
    {
        //-------------------------------------------------------
        //NB!!!!!!!!!!!!!!!!!!!!!!!!
        /// <summary>
        /// Set another atribute for time, which will 
        /// be combined to the date, and give the precise time when 
        /// the appointment is to occure.
        /// </summary>
        private EventDao eventDao;
        public RelayCommand CreateCommand { get; private set; }
        public string Name { get; set; }
        public string Venu { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public CreateEventViewModel()
        {
            CreateCommand = new RelayCommand(OnCreateEvent);
            eventDao = new EventDao("Event");
            Date = DateTime.Now;
        }

        private void OnCreateEvent()
        {
            Event newEvent = new Event(Name, Description, Venu, Date);

        }
    }
}
