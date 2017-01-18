using System;

namespace Dao.Entities
{
    public class TimeEntity : Entity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TimeNarative { get; set; }
        
        public TimeEntity(int id, string name, DateTime start, DateTime end, string type):base(name, id)
        {
            StartTime = start;
            EndTime = end;
            if(type == "Event")
            {
                TimeNarative = "This Appointment Occures At: "+EndTime.ToString();
            }
            else
            {
                TimeNarative = "This Working Session Occures At: " + StartTime.Date+" From "
                    +StartTime.TimeOfDay+" To "+EndTime.TimeOfDay;

            }
        }
    }
}
