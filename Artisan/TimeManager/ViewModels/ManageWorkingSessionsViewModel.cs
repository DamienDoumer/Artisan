using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dao.Entities;
using Dao;
using Artisan.MVVMShared;
using System.Collections.ObjectModel;

namespace TimeManager.ViewModels
{
    public class ManageWorkingSessionsViewModel : BindableBase
    {
        public static event Action CreateWorkingSessionCommand;
        public static event Action<WorkingSession> EditWorkingSession;
        public static event Action<WorkingSession, ObservableCollection<WorkingSession>> DeleteWorkingSession;

        private WorkingSession currentWorkingSession;
        private WorkingSessionDao wrkDao;
        private TaskDao taskDao;
        private ObservableCollection<Task> tasks;
        private ObservableCollection<WorkingSession> workingSessions;
        private OccuranceMonitor occuranceMon;
        private string grpBoxHeader;

        public string GroupboxHeader
        {
            get { return grpBoxHeader; }
            set { SetProperty(ref grpBoxHeader, value); }
        }
        public ObservableCollection<WorkingSession> WorkingSessions
        {
            get { return workingSessions; }
            set { SetProperty(ref workingSessions, value); }
        }
        public ObservableCollection<Task> Tasks
        {
            get { return tasks; }
            set { SetProperty(ref tasks, value); }
        }
        public WorkingSession CurrentWorkingSession
        {
            get { return currentWorkingSession; }
            set
            {
                SetProperty(ref currentWorkingSession, value);
                if(currentWorkingSession != null)
                {
                    Tasks = new ObservableCollection<Task>(currentWorkingSession.Tasks);
                }
                else
                {
                    Tasks = null;
                }
                DeleteCommand.RaiseCanExecuteChanged();
                EditCommand.RaiseCanExecuteChanged();
            }
        }
        public RelayCommand CreateCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand<string> SortWorkingSessionsCommand { get; private set; }

        public ManageWorkingSessionsViewModel()
        {
            CreateCommand = new RelayCommand(OnCreate);
            DeleteCommand = new RelayCommand(OnDelete, CanDelete);
            EditCommand = new RelayCommand(OnEdit, CanEdit);
            wrkDao = new WorkingSessionDao("WorkingSession");
            taskDao = new TaskDao("Task");
            tasks = new ObservableCollection<Task>();

            workingSessions = new ObservableCollection<WorkingSession>(
                wrkDao.RetrieveAllWorkingSessions());
            GroupboxHeader = "All your working Sessions";

            SortWorkingSessionsCommand = new RelayCommand<string>(OnSortWorkingSessions);
        }
        public void OnCreate()
        {
            CreateWorkingSessionCommand?.Invoke();
        }
        public void OnDelete()
        {
            DeleteWorkingSession?.Invoke(CurrentWorkingSession, WorkingSessions);
            //wrkDao.Delete(CurrentWorkingSession);
            //WorkingSessions.Remove(CurrentWorkingSession);
        }
        public bool CanDelete()
        {
            return CurrentWorkingSession != null;
        }
        public void OnEdit()
        {
            EditWorkingSession?.Invoke(CurrentWorkingSession);
        }
        public bool CanEdit()
        {
            return CurrentWorkingSession != null;
        }
        public void OnSortWorkingSessions(string mode)
        {
            List<WorkingSession> present;
            List<WorkingSession> future;
            List<WorkingSession> past;

            occuranceMon = OccuranceMonitor.Instance;
            occuranceMon.SortWorkingSessions(out present, out future, out past);

            switch (mode)
            {
                case "today":
                    addToEventsList(present);
                    GroupboxHeader = "Today's Working Sessions";
                    break;
                case "past":
                    GroupboxHeader = "Past Working Sessions";
                    addToEventsList(past);
                    break;
                case "future":
                    GroupboxHeader = "Future Working Sessions";
                    addToEventsList(future);
                    break;
            }
        }
        private void addToEventsList(List<WorkingSession> wrk)
        {
            WorkingSessions = new ObservableCollection<WorkingSession>(wrk);
        }
    }
}
