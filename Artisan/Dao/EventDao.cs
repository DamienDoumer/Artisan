using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using Dao.Entities;

namespace Dao
{
    public class EventDao : BaseDao
    {
        public static string CreateStatement { get; } = "CREATE TABLE `Event` (`ID`	INTEGER PRIMARY KEY AUTOINCREMENT, `Name`	VarChar(100), `Description`	TEXT NOT NULL, `Venue`	VarChar(100), `Date_Time`	VarChar(100))";

        public EventDao(string table) : base(table)
        {
        }

        public bool Save(Event data)
        {
            Event evt = data as Event;
            string attribs = CreateStringAttribute(evt.GetProperties());
            string attribVals = CreateStringAttributeValues(evt.GetPropertyValues());
            StringBuilder build = new StringBuilder();
            int success = 0;

            try
            {
                using (connection = new Connection())
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    SQLiteConnection con = connection.Open(DataSource);
                    
                    build.Append("Insert into ").Append(Table)
                        .Append(" " + attribs).Append(" Values " + attribVals);

                    command.Connection = con;
                    Query = build.ToString();
                    command.CommandText = Query;

                    success = command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            if (success == 1)
                return true;
            return false;
        }//Works
        public bool DeleteAllEvents()
        {
            return base.DeleteAll(CreateStatement);
        }//Works
        public bool Delete(Event evnt)
        {
            Event ev = evnt;
            return base.Delete(ev.ID);
        }//Works
        public List<Event> RetrieveAllEvents()
        {
            string query = "Select * from " + Table;
            Query = query;

            List<Event> events = new List<Event>();
            Event evnt;

            using (connection = new Connection())
                using (SQLiteCommand command = new SQLiteCommand())
            {
                SQLiteConnection con = connection.Open(DataSource);
                command.Connection = con;
                command.CommandText = query;
                command.ExecuteNonQuery();
                SQLiteDataReader reader = command.ExecuteReader();
                
                while(reader.Read())
                {
                    evnt = new Event(Convert.ToInt32(reader[0].ToString()), reader[2].ToString(), 
                        reader[1].ToString(), reader[3].ToString(), Convert.ToDateTime(reader[4].ToString()));
                    events.Add(evnt);
                }
                reader.Close();
            }

            return events;
        }//Works
    }
}
