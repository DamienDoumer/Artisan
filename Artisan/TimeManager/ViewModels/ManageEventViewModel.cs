using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artisan.MVVMShared;
using System.Collections.ObjectModel;
using Dao.Entities;
using Dao;

namespace TimeManager.ViewModels
{
    public class ManageEventViewModel : BindableBase
    {
        ObservableCollection<Event> events;
        EventDao evtDao;

        ObservableCollection<Event> Events
        {
            get { return events; }
            set { SetProperty(ref events, value); }
        }

        public ManageEventViewModel()
        {
            evtDao = new EventDao("Event");
            Events = new ObservableCollection<Event>(evtDao.RetrieveAllEvents());
        }
    }
}
