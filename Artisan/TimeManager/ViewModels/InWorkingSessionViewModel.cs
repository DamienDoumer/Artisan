using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artisan.MVVMShared;
using Dao.Entities;
using System.Diagnostics;

namespace TimeManager.ViewModels
{
    public class InWorkingSessionViewModel : BindableBase
    {
        private static WorkingSession mainWorkingSession;
        private string timeString;
        private long progressValue;

        public static event Action ProgressNeeded;

        /// <summary>
        /// Tells if the user can still tick tasks as done or not done.
        /// </summary>
        public bool EnableTaskTick { get; set; }
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
            CancleCommand = new RelayCommand(OnCancle);
            SaveCommand = new RelayCommand(OnSave);
            OccuranceMonitor.Instance.CounterTimeChanged += OnInstance_CounterTimeChanged;
        }

        private void OnInstance_CounterTimeChanged(int h, int m, int s, DateTime time, decimal percentage)
        {
            DispatchService.Invoke(new Action(() =>
            {
                Debug.WriteLine(percentage);
                TimeString = DateTime.Now.Hour+":"+DateTime.Now.Minute+":"+DateTime.Now.Second;
            }));
        }

        private void OnCancle()
        {

        }
        private void OnSave()
        {

        }
    }
}
