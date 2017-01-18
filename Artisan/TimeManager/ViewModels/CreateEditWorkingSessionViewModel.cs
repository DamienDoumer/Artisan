using System;
using Dao.Entities;
using Artisan.MVVMShared;
using Dao;
using System.Diagnostics;

namespace TimeManager.ViewModels
{
    public class CreateEditWorkingSessionViewModel : BindableBase
    {
        private static WorkingSession workingSession;
        public static event Action<WorkingSession> NextStepLaunched;

        public static string Mode { get; set; }
        public static string Title { get; set; }
        public static WorkingSession MainWorkingSession
        {
            get { return workingSession; }
            set { workingSession = value; }
        }
        public RelayCommand NextCommand { get; private set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public CreateEditWorkingSessionViewModel()
        {
            if(Mode == "Create")
            {
                Title = "Create A Working Session";
                workingSession = new WorkingSession();
            }
            else
            {
                Title = "Edit A Working Session";
            }

            NextCommand = new RelayCommand(OnNextClick, CanNextClick);
        }

        private void OnNextClick()
        {
            ///Set the appropriate date and send it to the next layer 
            /// as a working session event
            MainWorkingSession.EndTime = new DateTime(MainWorkingSession.Day.Year,
                                             MainWorkingSession.Day.Month, MainWorkingSession.Day.Day,
                                             MainWorkingSession.EndTime.Hour,
                                             MainWorkingSession.EndTime.Minute, 
                                             MainWorkingSession.EndTime.Second);
            MainWorkingSession.StartTime = new DateTime(MainWorkingSession.Day.Year,
                                             MainWorkingSession.Day.Month, MainWorkingSession.Day.Day,
                                             MainWorkingSession.StartTime.Hour,
                                             MainWorkingSession.StartTime.Minute,
                                             MainWorkingSession.StartTime.Second);
            ///I set the ID of this wrkingsession to correspond to that
            /// of the workingsession to be Saved.
            MainWorkingSession.ID = new WorkingSessionDao("WorkingSession") { }.RetrieveLastWorkingSession().ID + 1;

            NextStepLaunched?.Invoke(MainWorkingSession);
        }
        private bool CanNextClick()
        {
            return true;
        }
    }
}
