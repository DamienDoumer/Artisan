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
            CancleCommand = new RelayCommand(OnCancle);
            SaveCommand = new RelayCommand(OnSave);
            OccuranceMonitor.Instance.CounterTimeChanged += OnInstance_CounterTimeChanged;
        }

        private void OnInstance_CounterTimeChanged(int h, int m, int s, DateTime time, float percentage)
        {
            DispatchService.Invoke(new Action(() =>
            {
                TimeString = percentage.ToString();
                Debug.WriteLine(progressValue);
                progressValue++;
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
