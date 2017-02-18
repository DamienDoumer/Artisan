using System;
using Dao.Entities;
using Artisan.MVVMShared;
using Dao;
using System.Diagnostics;
using System.Text;

namespace TimeManager.ViewModels
{
    public class CreateEditWorkingSessionViewModel : BindableBase
    {
        public const string CREATE_MODE = "Create";
        public const string EDIT_MODE = "Edit";
        public const string WORKING_SESSION_MODE = "WorkingSession";

        StringBuilder errorMessage;
        private static WorkingSession workingSession;
        public static event Action<WorkingSession> NextStepLaunched;
        public static event Action<string> NotificationNeeded;

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
            if (!ErrorCheck())
            {
                BuilWorkingSessionTime();
                NextStepLaunched?.Invoke(MainWorkingSession);
            }
            else
            {
                NotificationNeeded?.Invoke(errorMessage.ToString());
            }
        }
        private void OnNextClickSave()
        {

            BuilWorkingSessionTime();

           
            if (!ErrorCheck())
            {
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
            else
            {
                NotificationNeeded?.Invoke(errorMessage.ToString());
            }

        }

        public bool ErrorCheck()
        {
            bool error = false;
            bool errorProp = false;
            errorMessage = new StringBuilder();

            if (MainWorkingSession.Name == "")
            {
                errorMessage.Append("You must input the working session's title and description.\n");
                errorProp = true;
                error = true;
            }
            if (MainWorkingSession.Description == "" && !errorProp)
            {
                errorMessage.Append("You must input the working session's title and description.\n");
                errorProp = true;
                error = true;
            }
            if (MainWorkingSession.EndTime.Ticks <= MainWorkingSession.StartTime.Ticks)
            {
                errorMessage.Append("Your working session's end time cannot be earlier than its start time.\n");
                error = true;
            }
            if (MainWorkingSession.StartTime.Ticks < DateTime.Now.Ticks)
            {
                errorMessage.Append("Your working session's start time must not be in the past.\n");
                error = true;
            }

            return error;
        }

        private void BuilWorkingSessionTime()
        {
            ///Set the appropriate date and send it to the next layer 
            /// as a working session event
            MainWorkingSession.EndTime = new DateTime(MainWorkingSession.Day.Year,
                                             MainWorkingSession.Day.Month, MainWorkingSession.Day.Day,
                                             EndTime.Hour,
                                             EndTime.Minute,
                                             EndTime.Second);
            MainWorkingSession.StartTime = new DateTime(MainWorkingSession.Day.Year,
                                             MainWorkingSession.Day.Month, MainWorkingSession.Day.Day,
                                             StartTime.Hour,
                                             StartTime.Minute,
                                             StartTime.Second);
        }
        private bool CanNextClick()
        {
            return true;
        }
    }
}
