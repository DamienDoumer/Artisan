using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        public WorkingSession() : base(0, "", DateTime.Now, DateTime.Now, "WorkingSession")
        {
        }

        public WorkingSession(string name, DateTime day, string desc,
            DateTime startT, DateTime endT) : base(0, name, startT, endT, "WorkingSession")
        {
            this.day = day.Date;
            Description = desc;
            StartTime = startT;
            EndTime = endT;
        }

        public WorkingSession(string name, DateTime day,
            List<Task> tasks,string desc, DateTime startT, DateTime endT)
            : base(0, name, startT, endT, "WorkingSession")
        {
            Description = desc;
            StartTime = startT;
            EndTime = endT;
            Tasks = tasks;
            /// The Day should be stored as a date without time.
            this.day = day.Date;
        }
        public WorkingSession(int id, string name, DateTime day,
            string desc, DateTime startT, DateTime endT) : base(id, name, startT, endT, "WorkingSession")
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

        public override bool Equals(object obj)
        {
            WorkingSession wrk = obj as WorkingSession;

            if(typeof(WorkingSession) == this.GetType() && wrk != null)
            {
                return wrk.ID == ID;
            }

            return false;
        }
    }
}
