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
        //string btnContent;

        //Changes when the button needs to be for cancle or save
        //public string ButtonContent
        //{
        //    get { return btnContent; }
        //    set { SetProperty(ref btnContent, value); }
        //}
        public static event Action<string> Notification;
        public static event Action<string> CancelWorkingSession;
        public static event Action WorkingSessionTerminated;

        bool canSave;
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
            Terminated = false;
            EnableTaskTick = true;
            //ButtonContent = "Cancle";
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
                  //ButtonContent = "Done";
                  Terminated = true;
                  EnableTaskTick = false;
                  Notification?.Invoke("The time for this working session is over.");
                  Debug.WriteLine(Terminated);
                  CancleCommand.RaiseCanExecuteChanged();
                  SaveCommand.RaiseCanExecuteChanged();
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
                CancelWorkingSession?.
                Invoke("Are you sure you want to abort this working session ? Your current progress will be saved.");
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
            return canSave;
        }
        private bool CanCancel()
        {
            if (Terminated)
            {
                canSave = true;
            }
            else
            {
                canSave = false;
            }
            return !Terminated;
        }
        public static void TerminateWoringSession()
        {
            WorkingSessionTerminated?.Invoke();
        }
    }
}
