using System;

namespace Dao.Entities
{
    public class TimeEntity : Entity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeEntity(int id, string name, DateTime start, DateTime end):base(name, id)
        {
            StartTime = start;
            EndTime = end;
        }
    }
}
