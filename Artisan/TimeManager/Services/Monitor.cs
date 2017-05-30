using Dao;
using Dao.ShareResources;
using TimeManager.Services;
using Dao.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        public WorkingSessionStatistic CalculateStatsForWorkingSession(WorkingSession wrk)
        {
            int accTasks = taskDao.RetrieveUnAccomplishedTasksCountForWorkingSession(wrk);
            int totalTasks = taskDao.RetrieveTasksForWorkingSession(wrk).Count;
            
            decimal percentage = CalculatePercentage(accTasks, totalTasks);
            return new WorkingSessionStatistic(wrk.Name, percentage, wrk.StartTime ,wrk.Tasks);
        }
        public List<WorkingSessionStatistic> CalculateStatsForEveryWorkingSessions()
        {
            var stats = new List<WorkingSessionStatistic>();
            foreach (WorkingSession wrk in workingSessionDao.RetrieveAllWorkingSessions())
            {
                stats.Add(CalculateStatsForWorkingSession(wrk));
            }

            return stats;
        }

        public void MonitorTimeSpentOnPC()
        {}
        public decimal CalculatePercentageOfAccomplishedTasks()
        {
            int numAccomplishedTasks = taskDao.RetrieveAccomplishedTasksCount();
            int totalTasks = taskDao.RetrieveAllTasksCount();

            return CalculatePercentage(numAccomplishedTasks, totalTasks);
        }
        public decimal CalculatePercentageOfUnAccomplishedTasks()
        {

            return CalculatePercentage(taskDao.RetrieveUnAccomplishedTasksCount()
                , taskDao.RetrieveAllTasksCount());
        }
        public decimal CalculatePercentageOfDoneWorkingSessions()
        {
            return CalculatePercentage(workingSessionDao.RetrieveAccommplishedWorkingSessionsCount(),
                workingSessionDao.RetrieveAllWorkingSessionsCount());
        }
        public decimal CalculatePercentageOfNotDoneWorkingSessions()
        {
            return CalculatePercentage(workingSessionDao.RetrieveNotAccommplishedWorkingSessionsCount(),
                workingSessionDao.RetrieveAllWorkingSessionsCount());
        }
        //Calculate the percentage of tasks and sessions completed together
        public decimal CalculateTotalComplisionPercentage()
        {
            var wrkPercent = CalculatePercentageOfDoneWorkingSessions();
            var taskPercent = CalculatePercentageOfAccomplishedTasks();
            return CalculatePercentage(wrkPercent + taskPercent, 200);
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
        private decimal CalculatePercentage(int small, int total)
        {
            decimal percentage;

            try
            {
                percentage = Decimal.Divide((decimal)small, (decimal)total);
                percentage = Decimal.Multiply(percentage, 100);
            }
            catch (DivideByZeroException)
            {
                return new decimal(0);
            }
            
            return percentage;
        }
        private decimal CalculatePercentage(decimal small, int total)
        {
            decimal percentage;

            try
            {
                percentage = Decimal.Divide(small, (decimal)total);
                percentage = Decimal.Multiply(percentage, 100);
            }
            catch(DivideByZeroException)
            {
                return new decimal(0);
            }

            return percentage;
        }
    }
}
