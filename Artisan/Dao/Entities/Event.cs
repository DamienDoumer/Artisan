using System;

namespace Dao.Entities
{
    public class Event : TimeEntity
    {
        private string venue;

        public string Description { get; set; }
        public string Venue
        {
            get
            {
                return venue;
            }
            set
            {
                if(value.Length > 50)
                {
                    ///----------------------------------///
                    /// translation needed               ///
                    /// ---------------------------------///
                    throw (new 
                        Exception("The venue of your event should not be of more than 100 characters."));
                }
                else
                {
                    venue = value;
                }
            }
        }
        public DateTime Date_Time { get; set; }

        public Event(string name, string desc, string venue, DateTime date)
            : base(0, name, DateTime.Now, date, "Event")
        {
            Description = desc;

            if (venue.Length > 50)
            {
                throw new Exception("The venue of your event should not be of more than 100 characters.");
            }
            else
            {
                this.venue = venue;
            }
            Date_Time = date;
        }
        public Event():base(0, "", DateTime.Now, DateTime.Now, "Event")
        {

        }
        public Event(int id, string name, string desc, string venue, DateTime date) 
            : base(id, name, DateTime.Now, date, "Event")
        {
            Description = desc;

            if (venue.Length > 50)
            {
                ///----------------------------------///
                /// translation needed               ///
                /// ---------------------------------///
                throw new Exception("The venue of your event should not be of more than 100 characters.");
            }
            else
            {
                this.venue = venue;
            }
            Date_Time = date;
        }

        public override string[] GetProperties()
        {
            return new string[] { "Description", "Name", "Venue", "Date_Time" };
        }
        public override string[] GetPropertyValues()
        {
            return new string[] { Description, Name, Venue, Date_Time.ToString() };
        }
        public override string ToString()
        {
            return Description +" "+ Name + " " + Venue + " " + Date_Time.ToString();
        }
    }
}
