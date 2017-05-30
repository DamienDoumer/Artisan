using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artisan.MVVMShared;
using System.Collections.ObjectModel;
using TimeManager.Services;
using Dao.Entities;
using TimeManager;
using System.Diagnostics;
using WpfCharts;
using System.Windows.Media;

namespace TimeManager.ViewModels
{
    public class ViewStatisticsViewModel : BindableBase
    {
        public ObservableCollection<ChartLine> ChatLines { get; set; }
        public string[] Axes
        {
            get; set;
        }
        Monitor monitor;
        ObservableCollection<WorkingSessionStatistic> workingSessionsStats;
        ObservableCollection<Task> tasks;
        WorkingSessionStatistic currentWorkingSessionStat;

        public ObservableCollection<WorkingSessionStatistic> WorkingSessionsStats
        {
            get { return workingSessionsStats; }
            set { SetProperty(ref workingSessionsStats, value); }
        }
        public ObservableCollection<Task> Tasks
        {
            get { return tasks; }
            set { SetProperty(ref tasks, value); }
        }
        public WorkingSessionStatistic CurrentWorkingSessionStat
        {
            get { return currentWorkingSessionStat; }
            set
            {
                SetProperty(ref currentWorkingSessionStat, value);
                if(currentWorkingSessionStat != null)
                {
                    Tasks = new ObservableCollection<Task>(currentWorkingSessionStat.Tasks);
                }
            }
        }

        public ViewStatisticsViewModel()
        {
            monitor = new Monitor();
            Tasks = new ObservableCollection<Task>();
            WorkingSessionsStats = new ObservableCollection<WorkingSessionStatistic>(monitor.CalculateStatsForEveryWorkingSessions());
            
            List<double> points = new List<double>();
            points.Add(Convert.ToDouble(monitor.CalculateTotalComplisionPercentage()));
            points.Add(Convert.ToDouble(monitor.CalculatePercentageOfDoneWorkingSessions()));
            points.Add(Convert.ToDouble(monitor.CalculatePercentageOfAccomplishedTasks()));
            points.Add(Convert.ToDouble(monitor.CalculatePercentageOfUnAccomplishedTasks()));
            points.Add(Convert.ToDouble(monitor.CalculatePercentageOfNotDoneWorkingSessions()));

            foreach(double val in points)
            {
                Debug.WriteLine(val);
            }

            ChatLines = new ObservableCollection<ChartLine>
            {
                new ChartLine
                {
                    LineColor = Colors.Blue,
                    LineThickness = 1,
                    PointDataSource = points,
                    FillColor = Colors.SkyBlue
                }
            };
            Axes = new string[] { "Total Acommplishments", "Done Working Sessions", "Accomplished Tasks",
                "Tasks Not Accomplished", "Working Sessions not Accomplished"};
        }
    }
}
