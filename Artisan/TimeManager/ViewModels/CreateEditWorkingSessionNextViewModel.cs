using System;
using Dao.Entities;
using Dao;
using Artisan.MVVMShared;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;

namespace TimeManager.ViewModels
{
    public class CreateEditWorkingSessionNextViewModel : BindableBase
    {
        public static event Action CreateEditTerminated;
        public static event Action SwitchToPreviousScreen;

        private WorkingSessionDao wrkDao;
        private TaskDao taskDao;
        private ObservableCollection<Task> tasks;
        private Task currentTask;
        private string name;
        private string buttonText;
        /// <summary>
        /// this is used to save the newly added tasks in the 
        /// Edit mode.
        /// </summary>
        private List<Task> newTasks;

        public static string Mode { get; set; }
        public static WorkingSession MainWorkingSession { get; set; }
        public ObservableCollection<Task> TaskList
        {
            get
            { return tasks; }
            set
            {
                SetProperty(ref tasks, value);
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string AddEditButtonText
        {
            get
            {
                return buttonText;
            }
            set
            {
                SetProperty(ref buttonText, value);
            }
        }
        public Task CurrentTask
        {
            get { return currentTask; }
            set
            {
                SetProperty(ref currentTask, value);
                RemoveCommand.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
                ///this will occure only if this class 
                /// is in edit mode.
                /// this is the logic for changing add button's content.
                if(Mode == CreateEditWorkingSessionViewModel.EDIT_MODE)
                {
                    if (currentTask != null)
                    {
                        Name = currentTask.Name;
                    }
                    AddEditButtonText = "Edit";
                }
            }
        }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand RemoveCommand { get; private set; }
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand PreviousScreenCommand { get; private set; }

        public CreateEditWorkingSessionNextViewModel()
        {
            AddEditButtonText = "Add";
            if (Mode == CreateEditWorkingSessionViewModel.CREATE_MODE)
            {
                TaskList = new ObservableCollection<Task>();
            }
            else
            {
                TaskList = new ObservableCollection<Task>(MainWorkingSession.Tasks);
            }

            newTasks = new List<Task>();
            name = "";
            taskDao = new TaskDao("Task");
            wrkDao = new WorkingSessionDao();
            SaveCommand = new RelayCommand(OnSave, CanSave);
            RemoveCommand = new RelayCommand(OnRemove, CanRemove);
            AddCommand = new RelayCommand(OnAddEdit, CanAdd);
            PreviousScreenCommand = new RelayCommand(onPreviousClicked);
        }

        public void onPreviousClicked()
        {
            SwitchToPreviousScreen?.Invoke();
        }
        private void OnSave()
        {
            if(Mode == CreateEditWorkingSessionViewModel.CREATE_MODE)
            {
                MainWorkingSession.Tasks = new List<Task>(TaskList);
                wrkDao.Save(MainWorkingSession);
                SaveTasks(MainWorkingSession.Tasks);
            }
            else
            {
                wrkDao.Update(MainWorkingSession);

                if (newTasks.Count > 0)
                {
                    ///In edit mode, save only the newly added tasks.
                    SaveTasks(newTasks);
                }
            }
            OccuranceMonitor.Instance.StartMonitoring(MainWorkingSession);

            CreateEditTerminated?.Invoke();
        }
        private bool CanSave()
        {
            return TaskList.Count > 0;
        }
        private void OnRemove()
        {
            if (Mode == CreateEditWorkingSessionViewModel.EDIT_MODE)
            {
                taskDao.Delete(CurrentTask);
            }

            Name = "";
            TaskList.Remove(CurrentTask);
        }
        private bool CanRemove()
        {
            return TaskList.Count > 0 && CurrentTask !=null;
        }
        /// <summary>
        /// This handles both the edit and add scenario
        /// deppending on if the user is creating or editing a task.
        /// </summary>
        private void OnAddEdit()
        {
            int index = 0;
            Task nTask;

            ///If the user is adding a task and not editing
            if (Name != "")
            {
                nTask = new Task(name, MainWorkingSession.ID, false);

                if (buttonText == "Add")
                {
                    TaskList.Add(nTask);
                    //add the new tasks to be saved only if the mode is Edit
                    if (Mode == CreateEditWorkingSessionViewModel.EDIT_MODE)
                    {
                        newTasks.Add(nTask);
                    }
                }
                else
                {
                    if (CurrentTask != null)
                    {
                        CurrentTask.Name = Name;
                        taskDao.Update(CurrentTask);
                        index = TaskList.IndexOf(CurrentTask);
                        TaskList.Add(CurrentTask);
                        TaskList.RemoveAt(index);
                        AddEditButtonText = "Add";
                    }
                }
            }
            else
            {
                AddEditButtonText = "Add";
            }

            Name = "";
        }
        private bool CanAdd()
        {
            return true;
        }
        private void SaveTasks(List<Task> tasks)
        {
            foreach (Task t in tasks)
            {
                if (MainWorkingSession.ID == 0)
                {
                    t.WorkingSessionID = 1;
                }
                else
                {
                    t.WorkingSessionID = MainWorkingSession.ID;
                }
                taskDao.Save(t);
            }
        }
    }
}
