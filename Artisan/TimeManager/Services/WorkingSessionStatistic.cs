using Dao.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeManager.Services
{
    public class WorkingSessionStatistic
    {
        public decimal Percentage { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public List<Task> Tasks { get; }

        public WorkingSessionStatistic(string title, decimal percentage, DateTime start, List<Task> tasks)
        {
            Tasks = tasks;
            StartTime = start;
            Title = title;
            Percentage = percentage;
        }
    }
}
