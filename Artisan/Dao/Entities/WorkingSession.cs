using System;
using System.Collections.Generic;

namespace Dao.Entities
{
    public class WorkingSession : TimeEntity
    {
        private DateTime day;
        public List<Task> Tasks { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// The Day should be stored as a date without time.
        /// </summary>
        public DateTime Day
        {
            get { return day; }
            set { day = value.Date; }
        }

        public WorkingSession(string name, DateTime day, string desc,
            DateTime startT, DateTime endT) : base(0, name, startT, endT)
        {
            this.day = day.Date;
            Description = desc;
            StartTime = startT;
            EndTime = endT;
        }

        public WorkingSession(string name, DateTime day,
            List<Task> tasks,string desc, DateTime startT, DateTime endT) : base(0, name, startT, endT)
        {
            Description = desc;
            StartTime = startT;
            EndTime = endT;
            Tasks = tasks;
            /// The Day should be stored as a date without time.
            this.day = day.Date;
        }
        public WorkingSession(int id, string name, DateTime day,
            string desc, DateTime startT, DateTime endT) : base(id, name, startT, endT)
        {
            Name = name;
            Description = desc;
            StartTime = startT;
            EndTime = endT;
            this.day = day.Date;
        }

        public override string[] GetProperties()
        {
            return new string[] { "Name", "Description", "Day", "StartTime", "EndTime" };
        }
        public override string[] GetPropertyValues()
        {
            return new string[] {Name, Description, Day.ToString(), StartTime.ToString(), EndTime.ToString() };
        }
        public override string ToString()
        {
            return Name +" "+ Description + " " + Day.ToString() + " " + StartTime.ToString() + " " + EndTime.ToString();
        }
    }
}
