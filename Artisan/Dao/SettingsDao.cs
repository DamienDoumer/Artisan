using System;
using Dao.Entities;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Dao
{
    public class SettingsDao : BaseDao
    {
        public static string CreateStatement { get; } = "CREATE TABLE `Settings` ( " +
                                " `AlarmSound`	TEXT NOT NULL, " +
                                "`TimeDifference` INTEGER NOT NULL " +
                                " );";

        public SettingsDao(string table) : base(table)
        {
        }

        public bool Save(Settings data)
        {
            return ChangeAlarmSoundPath(data.AlarmSound) && ChangeTimeDifference(data.TimeDifference);
        }//Works
        public bool ChangeAlarmSoundPath(string path)
        {
            return Update("AlarmSound", path);

        }//Works
        public bool ChangeTimeDifference(int minutes)
        {
            return Update("TimeDifference", minutes.ToString());
        }//Works
        public Settings RetrieveSettings()
        {
            Settings set = null;
            string query = "Select * from Settings;";
            Query = query;

            using (connection = new Connection())
            using (SQLiteCommand command = new SQLiteCommand())
            {
                Console.WriteLine(query);
                SQLiteConnection con = connection.Open(DataSource);
                command.Connection = con;
                command.CommandText = Query;
                command.ExecuteNonQuery();
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    set = new Settings(reader[0].ToString(), Convert.ToInt32(reader[1].ToString()));
                }
                reader.Close();
            }
            return set;
        }//Works
        public bool SaveDateForDaysOfWork()
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            try
            {
                ExecuteQuery("Insert into DaysOfWork (Day) values ('" + date.ToString() + "')");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }//works

        public List<DateTime> RetrieveDatesForDaysOfWork()
        {
            string query = "Select * from DaysOfWork";
            Query = query;
            List<DateTime> dates = new List<DateTime>();

            using (connection = new Connection())
            using (SQLiteCommand command = new SQLiteCommand())
            {
                Console.WriteLine(query);
                SQLiteConnection con = connection.Open(DataSource);
                command.Connection = con;
                command.CommandText = Query;
                command.ExecuteNonQuery();
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dates.Add(Convert.ToDateTime(reader[0].ToString()));
                }
                reader.Close();
            }

            return dates;

        }//works
    }
}
