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

        ManageWorkingSessionsViewModel manageWorkingSessionVM;
        CreateEditWorkingSessionNextViewModel createWrkNextVM;
        CreateEventViewModel createEventViewModel = new CreateEventViewModel();
        CreateEditWorkingSessionViewModel createEditWorkingSession;
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
            ///Workingsession management code
            CreateEditWorkingSessionViewModel.Mode = "Create";
            createEditWorkingSession = new CreateEditWorkingSessionViewModel();
            manageWorkingSessionVM = new ManageWorkingSessionsViewModel();
            CreateEditWorkingSessionViewModel.NextStepLaunched += OnNextStepSwitch;
            ManageWorkingSessionsViewModel.CreateWorkingSessionCommand += OnCreateWorkingSessionInitiated;
            CreateEditWorkingSessionNextViewModel.CreateEditTerminated += OnSwitchToManageWorkingSession;
            ManageWorkingSessionSwitchCommand = new RelayCommand(OnSwitchToManageWorkingSession);

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

        #region WorkingSession ManagementCode

        private void OnSwitchToManageWorkingSession()
        {
            CurrentViewModel = manageWorkingSessionVM;
        }
        /// <summary>
        /// Occures when the next button of create working session is pressed
        /// </summary>
        /// <param name="wrk"></param>
        private void OnNextStepSwitch(WorkingSession wrk)
        {
            CreateEditWorkingSessionNextViewModel.MainWorkingSession = wrk;
            CreateEditWorkingSessionNextViewModel.Mode = "Create";
            if(createWrkNextVM == null)
            {
                createWrkNextVM = new CreateEditWorkingSessionNextViewModel();
            }
            CurrentViewModel = createWrkNextVM;
        }
        private void OnCreateWorkingSessionInitiated()
        {
            CreateEditWorkingSessionNextViewModel.Mode = "Create";
            CurrentViewModel = createEditWorkingSession;
        }

        #endregion

        #region Event managing code
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
        #endregion
        
        private void OnSettingsViewSwitch()
        {

        }
        private void OnMessagesViewSwitch()
        {

        }
    }
}
