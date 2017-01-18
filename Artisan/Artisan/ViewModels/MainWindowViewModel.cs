using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.ViewModels;
using Artisan.MVVMShared;
using System.Diagnostics;
using Dao.Entities;
using TimeManager;

namespace Artisan.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// These commands are to handle the click events of the 
        /// Hamburger Menu.
        /// </summary>
        public RelayCommand MainMenuSwitchCommand { get; private set; }
        public RelayCommand ManageEventSwitchCommand { get; private set; }
        public RelayCommand ManageWorkingSessionSwitchCommand { get; private set; }

        CreateEventViewModel createEventViewModel = new CreateEventViewModel();
        MainMenuViewModel mainMenuViewModel = new MainMenuViewModel();
        ManageEventViewModel manageEventViewModel;
        private BindableBase currentViewModel;
        private OccuranceMonitor occuranceMon;

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }


        public MainWindowViewModel()
        {
            manageEventViewModel = new ManageEventViewModel();
            ///Here I handle the event Fired when the user wants to edit an event.
            ManageEventViewModel.EditEvent += NavigateToEditEventView;
            ManageEventViewModel.CreateEvent += NavigateToCreateEventView;
            CreateEventViewModel.EventCreated += OnEventCreated;
            MainMenuSwitchCommand = new RelayCommand(OnMainMenuViewSwitch);
            ManageEventSwitchCommand = new RelayCommand(OnManageEventViewSwitch);
            CurrentViewModel = mainMenuViewModel;

            ///Code for monitoring working sessions and events.
            occuranceMon = OccuranceMonitor.Instance;
            occuranceMon.SortTimeEntities();
            occuranceMon.StartMonitoring();
        }

        private void OnEventCreated(Event evt)
        {
            OnManageEventViewSwitch();
            occuranceMon.StartMonitoring(evt);
        }
        private void NavigateToCreateEventView()
        {
            CreateEventViewModel.Title = "Create Event";
            CurrentViewModel = createEventViewModel;
        }
        private void NavigateToEditEventView(Event evt)
        {
            CreateEventViewModel.MainEvent = evt;
            CreateEventViewModel.Title = "Edit Event";
            CurrentViewModel = createEventViewModel;
        }
        private void OnMainMenuViewSwitch()
        {
            CurrentViewModel = mainMenuViewModel;
        }
        private void OnManageEventViewSwitch()
        {
            CurrentViewModel = manageEventViewModel;
        }
        private void OnManageWorkingSessionViewSwitch()
        {

        }
        private void OnSettingsViewSwitch()
        {

        }
        private void OnMessagesViewSwitch()
        {

        }
    }
}
