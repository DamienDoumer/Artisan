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

        /// <summary>
        /// Fired when the user chooses an event which he wants to edit 
        /// ----------------------------------------------------------
        /// NB: Making this event static, allows me to access it freely in a subsequent layer,
        /// Like in the MainViewModel.
        /// </summary>
        /// <param name="evt">The event to be edited.</param>
        public static event Action<Event> EditEvent;
        public static event Action CreateEvent;

        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand CreateEventCommand { get; private set; }

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
                EditCommand.RaiseCanExecuteChanged();
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
            CreateEventCommand = new RelayCommand(OnCreateEvent);
        }

        public void OnEditCommand()
        {
            Debug.WriteLine(EditEvent == null);
            EditEvent?.Invoke(CurrentEvent);
        }
        private bool CanEdit()
        {
            return CurrentEvent != null;
        }
        private void OnDelete()
        {
            evtDao.Delete(CurrentEvent);
            Events.Remove(CurrentEvent);
        }
        private bool CanDelete()
        {
            return CurrentEvent!=null;
        }
        private void OnCreateEvent()
        {
            CreateEvent?.Invoke();
        }
    }
}
