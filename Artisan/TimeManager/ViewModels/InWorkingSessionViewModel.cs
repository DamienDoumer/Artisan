using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artisan.MVVMShared;
using Dao.Entities;
using System.Diagnostics;
using Dao;

namespace TimeManager.ViewModels
{
    public class InWorkingSessionViewModel : BindableBase
    {
        private static WorkingSession mainWorkingSession;
        private string timeString;
        private long progressValue;
        private bool enableTaskTick;

        public static event Action<string> Notification;
        public static event Action<string> CancelWorkingSession;
        public static event Action WorkingSessionTerminated;

        /// <summary>
        /// WorkingSession terminated
        /// </summary>
        public static bool Terminated { get; set; }
        /// <summary>
        /// Tells if the user can still tick tasks as done or not done.
        /// </summary>
        public bool EnableTaskTick
        {
            get { return enableTaskTick; }
            set { SetProperty(ref enableTaskTick, value); }
        }
        public long ProgressValue
        {
            get { return progressValue; }
            set { SetProperty(ref progressValue, value); }
        }
        public string TimeString
        {
            get { return timeString; }
            set { SetProperty(ref timeString, value); }
        }

        public RelayCommand CancleCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public static WorkingSession MainWorkingSession
        {
            get
            {
                return mainWorkingSession;
            }
            set { mainWorkingSession = value; }
        }

        public InWorkingSessionViewModel()
        {
            EnableTaskTick = true;
            CancleCommand = new RelayCommand(OnCancle, CanCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
            OccuranceMonitor.Instance.CounterTimeChanged += OnInstance_CounterTimeChanged;
            OccuranceMonitor.Instance.CounterEnded += OnInstance_CounterEnded;
        }

        private void OnInstance_CounterEnded(DateTime time, WorkingSession wrk)
        {
            EnableTaskTick = false;

            if (!Terminated)
            {
                DispatchService.Invoke(new Action(() =>
              {
                  Terminated = true;
                  EnableTaskTick = false;
                  Notification?.Invoke("The time for this working session is over.");
                  SaveCommand.RaiseCanExecuteChanged();
                  CancleCommand.RaiseCanExecuteChanged();
                  Terminated = false;
              }));
            }
        }

        private void OnInstance_CounterTimeChanged(int h, int m, int s, DateTime time, decimal percentage)
        {
            DispatchService.Invoke(new Action(() =>
            {
                TimeString = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            }));
        }

        private void OnCancle()
        {
            DispatchService.Invoke(new Action(() =>
            {
                CancelWorkingSession?.Invoke("Are you sure you want to abort this working session ? Your current progress will be saved.");
                //Save();
                ///The saving working sessions process is found in the MainWindow View code, 
                ///This is bad programming due to time constraints

            }));
        }

        private void OnSave()
        {
            DispatchService.Invoke(new Action(() =>
            {
                Save();
                WorkingSessionTerminated?.Invoke();
            }));
        }

        private void Save()
        {
            new WorkingSessionDao() { }.SaveAsDoneWorkingSession(MainWorkingSession);

            foreach (Task t in MainWorkingSession.Tasks)
            {
                new TaskDao("Task") { }.Update(t);
            }
        }

        private bool CanSave()
        {
            return Terminated;
        }
        private bool CanCancel()
        {
            return !Terminated;
        }
        public static void TerminateWoringSession()
        {
            WorkingSessionTerminated?.Invoke();
        }
    }
}
