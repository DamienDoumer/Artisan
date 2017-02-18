using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TimeManager.ViewModels;
using Artisan.MVVMShared;
using System.Diagnostics;
using Dao.Entities;
using TimeManager;
using System.Collections.ObjectModel;
using Artisan.Views;
using Dao;
using Artisan.Views.Notifications;
using Artisan.Services;

namespace Artisan.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// This tells the mode in which theis ViewModel is set for currently
        /// This depends on the scenarios in which the application is found.
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// notification objects.
        /// And sound player 
        /// </summary>
        GrowlNotifications growlNot;
        NotificationSoundPlayer player;

        /// <summary>
        /// Fired when th eneed of displaying a message to the user comes.
        /// </summary>
        public static event Action<string> DiaologNeeded;
        /// <summary>
        /// Fired before the user deletes a timeEntity
        /// </summary>
        public static event Action<TimeEntity, object> DeleteReponse;
        public static event Action<string, string, bool> DisplayTimeArrivedNotification;
        public static event Action<string, string> DisplayNotificationMessageBox;
        public static event Action<string> CancelWorkingSession;

        /// <summary>
        /// These commands are to handle the click events of the 
        /// Hamburger Menu.
        /// </summary>
        public RelayCommand MainMenuSwitchCommand { get; private set; }
        public RelayCommand ManageEventSwitchCommand { get; private set; }
        public RelayCommand ManageWorkingSessionSwitchCommand { get; private set; }
        public RelayCommand MessageSwitchCommand { get; private set; }

        ManageWorkingSessionsViewModel manageWorkingSessionVM;
        CreateEditWorkingSessionNextViewModel createWrkNextVM;
        CreateEventViewModel createEventViewModel = new CreateEventViewModel();
        CreateEditWorkingSessionViewModel createEditWorkingSession;
        MainMenuViewModel mainMenuViewModel = new MainMenuViewModel();
        ManageEventViewModel manageEventViewModel;
        private BindableBase currentViewModel;
        //MessengerViewModel messengerViewModel;
        private OccuranceMonitor occuranceMon;

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

        public MainWindowViewModel()
        {
            ///Instantiate notification object to notify the user when needed
            growlNot = new GrowlNotifications();
            player = new NotificationSoundPlayer(@"C:\Users\Damien\Desktop\Software development\MainProjectFolder\Artisan\Artisan\Resources\NotificationSound.wav");

            ///Workingsession management code
            CreateEditWorkingSessionNextViewModel.SwitchToPreviousScreen += SwitchToPreviousScreenInitiated;
            CreateEditWorkingSessionViewModel.Mode = CreateEditWorkingSessionViewModel.CREATE_MODE;
            createEditWorkingSession = new CreateEditWorkingSessionViewModel();
            manageWorkingSessionVM = new ManageWorkingSessionsViewModel();
            CreateEditWorkingSessionViewModel.NextStepLaunched += OnNextStepSwitch;
            ManageWorkingSessionsViewModel.CreateWorkingSessionCommand += OnCreateWorkingSessionInitiated;
            CreateEditWorkingSessionNextViewModel.CreateEditTerminated += OnSwitchToManageWorkingSession;
            ManageWorkingSessionSwitchCommand = new RelayCommand(OnSwitchToManageWorkingSession);
            ManageWorkingSessionsViewModel.EditWorkingSession += OnEditWorkingSessionInitiated;
            ManageWorkingSessionsViewModel.DeleteWorkingSession += DeleteWorkingSession;
            InWorkingSessionViewModel.CancelWorkingSession += InWorkingSessionViewModelCancelWorkingSession;
            InWorkingSessionViewModel.WorkingSessionTerminated += InWorkingSessionViewModel_WorkingSessionTerminated;
            InWorkingSessionViewModel.Notification += InWorkingSessionViewModel_Notification;
            CreateEditWorkingSessionViewModel.NotificationNeeded += OnCreateEditWorkingSessionViewModel_NotificationNeeded;

            ///Here I handle the event Fired when the user wants to edit an event.
            manageEventViewModel = new ManageEventViewModel();
            ManageEventViewModel.EditEvent += NavigateToEditEventView;
            ManageEventViewModel.CreateEvent += NavigateToCreateEventView;
            CreateEventViewModel.EventCreated += OnEventCreated;
            MainMenuSwitchCommand = new RelayCommand(OnMainMenuViewSwitch);
            ManageEventSwitchCommand = new RelayCommand(OnManageEventViewSwitch);
            CurrentViewModel = mainMenuViewModel;
            ManageEventViewModel.DeleteEvent += OnDeleteEvent;
            CreateEventViewModel.NotificationNeeded += OnCreateEventViewModel_NotificationNeeded;

            ///Code for monitoring working sessions and events.
            occuranceMon = OccuranceMonitor.Instance;
            occuranceMon.AlarmTimeArrived += OccuranceMon_AlarmTimeArrived;
            occuranceMon.SortTimeEntities();
            occuranceMon.StartMonitoring();
            occuranceMon.CounterStarted += OnOccuranceMon_CounterStarted;
            occuranceMon.CounterEnded += OnOccuranceMon_CounterEnded;

            ///Message Managing Code
            MessageSwitchCommand = new RelayCommand(OnMessagesViewSwitch);
            //messengerViewModel = new MessengerViewModel();
        }





        #region Occurance Monitor Handling code


        private void OnOccuranceMon_CounterEnded(DateTime time, WorkingSession wrk)
        {
            Mode = string.Empty;
        }
        /// <summary>
        /// Fired when the working session starts
        /// </summary>
        /// <param name="session"></param>
        /// <param name="time"></param>
        private void OnOccuranceMon_CounterStarted(WorkingSession session, DateTime time)
        {
            InWorkingSessionViewModel.MainWorkingSession = session;
            Mode = CreateEditWorkingSessionViewModel.WORKING_SESSION_MODE;
            CurrentViewModel = new InWorkingSessionViewModel();
        }
        private void OccuranceMon_AlarmTimeArrived(TimeEntity entity)
        {
            if (entity.GetType() == new WorkingSession().GetType())
            {
                WorkingSession wrk = entity as WorkingSession;
                ///True for working sessions
                DisplayTimeArrivedNotification?.Invoke(wrk.Name, wrk.Description, true);
                player.Player.Play();
            }
            else
            {
                Event evt = entity as Event;
                ///False for event 
                DisplayTimeArrivedNotification?.Invoke(evt.Name, evt.Description, false);
                growlNot.AppointmentTimeArrivedNotification("Appointment now",
                    evt.Name);
                player.Player.Play();
            }
        }



        #endregion

        #region WorkingSession ManagementCode

        private void OnCreateEditWorkingSessionViewModel_NotificationNeeded(string obj)
        {
            DiaologNeeded?.Invoke(obj);
        }

        private void InWorkingSessionViewModel_Notification(string obj)
        {
            DiaologNeeded?.Invoke(obj);
        }

        private void InWorkingSessionViewModel_WorkingSessionTerminated()
        {
            Mode = "";
            CurrentViewModel = manageWorkingSessionVM;
        }

        private void InWorkingSessionViewModelCancelWorkingSession(string obj)
        {
            CancelWorkingSession?.Invoke(obj);
        }

        /// <summary>
        /// Call deletion in the main window Since the Dialog cannot be called in
        /// ViewModel.
        /// </summary>
        /// <param name="wrk"></param>
        /// <param name="wrkList"></param>
        private void DeleteWorkingSession(WorkingSession wrk, ObservableCollection<WorkingSession> wrkList)
        {
            DeleteReponse?.Invoke(wrk, wrkList);
        }
        private void SwitchToPreviousScreenInitiated()
        {
            CurrentViewModel = new CreateEditWorkingSessionViewModel();
        }

        private void OnSwitchToManageWorkingSession()
        {
            ///If the user is not on a current working session
            if (Mode != CreateEditWorkingSessionViewModel.WORKING_SESSION_MODE)
            {
                CurrentViewModel = manageWorkingSessionVM;
            }
            else
            {
                ///Signal the usr that navigation has been disabled.
                NavigationDisabledSignal();
            }
        }
        /// <summary>
        /// Occures when the next button of create working session is pressed
        /// </summary>
        /// <param name="wrk"></param>
        private void OnNextStepSwitch(WorkingSession wrk)
        {
            CreateEditWorkingSessionNextViewModel.MainWorkingSession = wrk;

            if (CreateEditWorkingSessionViewModel.Mode == CreateEditWorkingSessionViewModel.CREATE_MODE)
            {
                CreateEditWorkingSessionNextViewModel.Mode = CreateEditWorkingSessionViewModel.CREATE_MODE;
                if (createWrkNextVM == null)
                {
                    createWrkNextVM = new CreateEditWorkingSessionNextViewModel();
                }
            }
            else
            {
                CreateEditWorkingSessionNextViewModel.Mode = CreateEditWorkingSessionViewModel.EDIT_MODE;
                if (createWrkNextVM == null)
                {
                    createWrkNextVM = new CreateEditWorkingSessionNextViewModel();
                }
            }

            CurrentViewModel = createWrkNextVM;

            ///--------------------------------
            /// Translation.......
            /// -------------------------------
            DiaologNeeded?.Invoke("Input a step at a time and press \"Add\" to add the step to the working session.");
        }
        private void OnCreateWorkingSessionInitiated()
        {
            CreateEditWorkingSessionViewModel.Mode = CreateEditWorkingSessionViewModel.CREATE_MODE;
            CreateEditWorkingSessionViewModel.MainWorkingSession = new WorkingSession();
            CreateEditWorkingSessionViewModel.MainWorkingSession.Day = DateTime.Now;
            CurrentViewModel = new CreateEditWorkingSessionViewModel();
        }
        private void OnEditWorkingSessionInitiated(WorkingSession wrk)
        {
            CreateEditWorkingSessionViewModel.Mode = CreateEditWorkingSessionViewModel.EDIT_MODE;
            CreateEditWorkingSessionViewModel.MainWorkingSession = wrk;
            CurrentViewModel = new CreateEditWorkingSessionViewModel();
        }



        #endregion

        #region Event managing code


        private void OnCreateEventViewModel_NotificationNeeded(string obj)
        {
            DiaologNeeded?.Invoke(obj);
        }

        private void OnDeleteEvent(Event evt, ObservableCollection<Event> evts)
        {
            DeleteReponse?.Invoke(evt, evts);
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

        private void OnManageEventViewSwitch()
        {
            if (Mode != CreateEditWorkingSessionViewModel.WORKING_SESSION_MODE)
            {
                CurrentViewModel = manageEventViewModel;
            }
            else
            {
                NavigationDisabledSignal();
            }
        }
        #endregion


        private void OnMainMenuViewSwitch()
        {
            if (Mode != CreateEditWorkingSessionViewModel.WORKING_SESSION_MODE)
            {
                CurrentViewModel = mainMenuViewModel;
            }
            else
            {
                NavigationDisabledSignal();
            }
        }
        private void OnSettingsViewSwitch()
        {
            if (Mode != CreateEditWorkingSessionViewModel.WORKING_SESSION_MODE)
            {

            }
        }
        private void OnMessagesViewSwitch()
        {
            if (Mode != CreateEditWorkingSessionViewModel.WORKING_SESSION_MODE)
            {
                //CurrentViewModel = messengerViewModel;
            }
            else
            {
                NavigationDisabledSignal();
            }
        }

        /// <summary>
        /// Signals the View to display notifications.
        /// </summary>
        private void NavigationDisabledSignal()
        {
            ///===============================================
            /// Translation...
            /// ==============================================
            DisplayNotificationMessageBox?.Invoke("Warning",
                "You have an ongoing working session, you are advised to focus on it before it ends.");
        }
    }
}
