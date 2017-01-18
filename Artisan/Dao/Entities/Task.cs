using System;

namespace Dao.Entities
{
    public class Task : Entity
    {
        public int WorkingSessionID { get; set; }
        public bool Accomplished { get; set; }

        public Task():base("", 0)
        {
        }
        public Task(int id , string name, int workingSessionID, bool accomp):base(name, id)
        {
            WorkingSessionID = workingSessionID;
            Accomplished = accomp;
        }
        public Task(string name, int workingSessionID, bool accomp) : base(name, 0)
        {
            Accomplished = accomp;
            WorkingSessionID = workingSessionID;
        }
        public override string[] GetProperties()
        {
            return new string[] { "Name", "WorkingSessionID", "Accomplished"};
        }
        public override string[] GetPropertyValues()
        {
            return new string[] { Name, WorkingSessionID.ToString(), Accomplished.ToString() };
        }
        public override string ToString()
        {
            return Name +" "+WorkingSessionID+" "+ Accomplished;
        }
    }
}
