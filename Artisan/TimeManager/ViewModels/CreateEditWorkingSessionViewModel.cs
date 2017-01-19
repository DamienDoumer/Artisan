using System;
using Dao.Entities;
using Artisan.MVVMShared;
using Dao;
using System.Diagnostics;

namespace TimeManager.ViewModels
{
    public class CreateEditWorkingSessionViewModel : BindableBase
    {
        public const string CREATE_MODE = "Create";
        public const string EDIT_MODE = "Edit";

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
            if(Mode == CREATE_MODE)
            {
                Title = "Create A Working Session";
                NextCommand = new RelayCommand(OnNextClickSave, CanNextClick);
            }
            else
            {
                Title = "Edit A Working Session";
                NextCommand = new RelayCommand(OnNextClickUpdate, CanNextClick);
            }
            if(MainWorkingSession == null)
            {
                MainWorkingSession = new WorkingSession();
                MainWorkingSession.Day = DateTime.Now;
            }
            EndTime = DateTime.Now;
            StartTime = DateTime.Now;
        }

        private void OnNextClickUpdate()
        {
            BuilWorkingSessionTime();
            NextStepLaunched?.Invoke(MainWorkingSession);
        }
        private void OnNextClickSave()
        {
            BuilWorkingSessionTime();
            ///I set the ID of this wrkingsession to correspond to that
            /// of the workingsession to be Saved.
            try
            {
                MainWorkingSession.ID = new WorkingSessionDao("WorkingSession") { }
                .RetrieveLastWorkingSession().ID + 1;
            }
            catch
            {
                MainWorkingSession.ID = 0;
            }

            NextStepLaunched?.Invoke(MainWorkingSession);
        }
        private void BuilWorkingSessionTime()
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
        }
        private bool CanNextClick()
        {
            return true;
        }
    }
}
