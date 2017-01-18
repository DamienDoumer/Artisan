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
using TimeManager;

namespace TimeManager.ViewModels
{
    public class ManageEventViewModel : BindableBase
    {
        ObservableCollection<Event> events;
        EventDao evtDao;
        private Event evnt;
        OccuranceMonitor occuranceMon;

        /// <summary>
        /// Fired when the user chooses an event which he wants to edit 
        /// ----------------------------------------------------------
        /// NB: Making this event static, allows me to access it freely in a subsequent layer,
        /// Like in the MainViewModel.
        /// </summary>
        /// <param name="evt">The event to be edited.</param>
        public static event Action<Event> EditEvent;
        public static event Action CreateEvent;
        private string groupboxHeader;

        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand CreateEventCommand { get; private set; }
        /// <summary>
        /// Loads events according to present, future, past
        /// </summary>
        public RelayCommand<string> LoadEventOfPeriodCommand { get; private set; }


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
        /// <summary>
        /// This is the header which is displayed on the group box containing the listview
        /// </summary>
        public string GroupboxHeader
        {
            get { return groupboxHeader; } set { SetProperty(ref groupboxHeader, value); }
        }

        public ManageEventViewModel()
        {
            groupboxHeader = "All Your Appointments";
            LoadEventOfPeriodCommand = new RelayCommand<string>(OnLoadEvent);
            evtDao = new EventDao("Event");
            Events = new ObservableCollection<Event>(evtDao.RetrieveAllEvents());
            EditCommand = new RelayCommand(OnEditCommand, CanDeleteOrEdit);
            DeleteCommand = new RelayCommand(OnDelete, CanDeleteOrEdit);
            CreateEventCommand = new RelayCommand(OnCreateEvent);
        }

        public void OnEditCommand()
        {
            EditEvent?.Invoke(CurrentEvent);
        }
        private void OnDelete()
        {
            evtDao.Delete(CurrentEvent);
            Events.Remove(CurrentEvent);
        }
        private bool CanDeleteOrEdit()
        {
            ///Sets the button to enabled only and only if 
            /// An event was choosen from the list.
            return CurrentEvent!=null;
        }
        private void OnCreateEvent()
        {
            CreateEvent?.Invoke();
        }
        private void OnLoadEvent(string period)
        {
            List<Event> present;
            List<Event> future;
            List<Event> past;

            occuranceMon = OccuranceMonitor.Instance;
            occuranceMon.SortEvents(out present, out future, out past);

            switch(period)
            {
                case "present":
                    addToEventsList(present);
                    GroupboxHeader = "Today's Appointments.";
                    break;
                case "past":
                    addToEventsList(past);
                    GroupboxHeader = "Past Appointments";
                    break;
                case "future":
                    GroupboxHeader = "Future Appointments";
                    addToEventsList(future);
                    break;
            }
        }
        private void addToEventsList(List<Event> evts)
        {
            Events = new ObservableCollection<Event>(evts);
        }
    }
}
