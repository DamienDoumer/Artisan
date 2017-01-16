using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artisan.MVVMShared;
using System.Collections.ObjectModel;
using Dao.Entities;
using Dao;
using System.Diagnostics;

namespace TimeManager.ViewModels
{
    public class ManageEventViewModel : BindableBase
    {
        ObservableCollection<Event> events;
        EventDao evtDao;
        private Event evnt;

        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }

        /// <summary>
        /// this is the selected Event
        /// </summary>
        public Event CurrentEvent
        {
            get
            { return evnt; }
            set
            {
                SetProperty(ref evnt, value);
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }
        public ObservableCollection<Event> Events
        {
            get { return events; }
            set { SetProperty(ref events, value); }
        }

        public ManageEventViewModel()
        {
            evtDao = new EventDao("Event");
            Events = new ObservableCollection<Event>(evtDao.RetrieveAllEvents());
            EditCommand = new RelayCommand(OnEditCommand, CanEdit);
            DeleteCommand = new RelayCommand(OnDelete, CanDelete);
        }

        public void OnEditCommand()
        {

        }
        private bool CanEdit()
        {
            return true; 
        }
        private void OnDelete()
        {
            Debug.WriteLine(CurrentEvent.ID);
            Debug.WriteLine(CurrentEvent.Name);
            evtDao.Delete(CurrentEvent);
            Events.Remove(CurrentEvent);
        }
        private bool CanDelete()
        {
            return CurrentEvent!=null;
        }
    }
}
