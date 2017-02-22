using Dao;
using Dao.ShareResources;

namespace TimeManager
{
    /// <summary>
    /// Calculates the statistics for the user's performances
    /// </summary>
    public class Monitor
    {
        public OccuranceMonitor OccuranceMon { get; }
        private SettingsDao settingsDao;
        private TaskDao taskDao;
        private WorkingSessionDao workingSessionDao;

        public Monitor()
        {
            OccuranceMon = OccuranceMonitor.Instance;
            //Save this day as a day if it is saved already, no PB.
            settingsDao = new SettingsDao("Settings");
            settingsDao.SaveDateForDaysOfWork();
            taskDao = new TaskDao(Shared.TaskTableName);
            workingSessionDao = new WorkingSessionDao();
        }

        public void MonitorTimeSpentOnPC()
        {}
        public float CalculatePercentageOfAccomplishedTasks()
        {
            int numAccomplishedTasks = taskDao.RetrieveAccomplishedTasksCount();
            int totalTasks = taskDao.RetrieveAllTasksCount();

            return CalculatePercentage(numAccomplishedTasks, totalTasks);
        }
        public float CalculatePercentageOfUnAccomplishedTasks()
        {

            return CalculatePercentage(taskDao.RetrieveUnAccomplishedTasksCount()
                , taskDao.RetrieveAllTasksCount());
        }
        public float CalculatePercentageOfDoneWorkingSessions()
        {
            return CalculatePercentage(workingSessionDao.RetrieveAccommplishedWorkingSessionsCount(),
                workingSessionDao.RetrieveAllWorkingSessionsCount());
        }
        public float CalculatePercentageOfNotDoneWorkingSessions()
        {
            return CalculatePercentage(workingSessionDao.RetrieveNotAccommplishedWorkingSessionsCount(),
                workingSessionDao.RetrieveAllWorkingSessionsCount());
        }
        public int NumberOfAccomplishedTasks()
        {
            return new int();
        }
        public int NumberOfUnAccomplishedTasks()
        {
            return new int();
        }
        public int NumberOfDoneWorkingSessions()
        {
            return new int();
        }
        private float CalculatePercentage(int small, int total)
        {
            return (small * 100) / total;
        }
    }
}
