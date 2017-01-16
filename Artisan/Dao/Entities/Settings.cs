using System;

namespace Dao.Entities
{
    public class Settings
    {
        public string AlarmSound { get; set; }
        public int TimeDifference { get; set; }
        public Settings(string alarmSound, int diff)
        {
            AlarmSound = alarmSound;
            TimeDifference = diff;
        }

        public string[] GetProperties()
        {
            return new string[] { "AlarmSound", "TimeDifference" };
        }
        public string[] GetPropertyValues()
        {
            return new string[] { AlarmSound, TimeDifference.ToString() };
        }
        public override string ToString()
        {
            return AlarmSound+" "+ TimeDifference;
        }
    }
}
