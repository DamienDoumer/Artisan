﻿using System;
using System.Collections.Generic;
using System.Text;
using Dao.Entities;
using System.Data.SQLite;
using System.Diagnostics;

namespace Dao
{
    public class WorkingSessionDao : BaseDao
    {
        TaskDao tDao;
        public static string CreateStatement { get; } = "CREATE TABLE \"WorkingSession\"(" +
                                    "`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                                    "`Name`	VarChar(50) NOT NULL," +
                                    "`Description`	TEXT NOT NULL," +
                                    "`Day`	VarChar(50) NOT NULL," +
                                    "`StartTime`	VarChar(50) NOT NULL," +
                                    "`EndTime`	VarChar(50) NOT NULL, " +
                                    "`Accomplished`	boolean NOT NULL"+
                                    ");";



        public WorkingSessionDao():base("WorkingSession")
        { tDao = new TaskDao("Task"); }



        public bool DeleteAllWorkingSessions()
        {
            return base.DeleteAll(CreateStatement);
        }//Works 
        public bool Delete(WorkingSession wrk)
        {
            tDao.DeleteTasksForWorkingSession(wrk);
            return base.Delete(wrk.ID);
        }//Works
        public bool Save(WorkingSession data)
        {
            string attribs = CreateStringAttribute(data.GetProperties());
            string attribVals = CreateStringAttributeValues(data.GetPropertyValues());
            StringBuilder build = new StringBuilder();
            int success = 0;

            build.Append("Insert into ").Append(Table)
                        .Append(" " + attribs).Append(" Values " + attribVals);

            success = ExecuteQuery(build.ToString());

            if (success == 1)
                return true;
            return false;
        }//Works
        public List<WorkingSession> RetrieveAllWorkingSessions()
        {
            Query = "Select * from " + Table;
            return RetrieveWorkingSessions(Query);
        }//Works
        public List<WorkingSession> RetrieveAccommplishedWorkingSessions()
        {
            return RetrieveWorkingSessions("select * from WorkingSession where Accomplished = 'True'");
        }//works
        public List<WorkingSession> RetrieveNotAccommplishedWorkingSessions()
        {
            return RetrieveWorkingSessions("select * from WorkingSession where Accomplished = 'False'");
        }//works
        //public List<WorkingSession> RetrieveAccommplishedBeforeTimeWorkingSessions()
        //{
        //    return RetrieveWorkingSessions("...");
        //}
        public List<WorkingSession> RetrieveWorkingSessionsForDate(DateTime date)
        {
            date = date.Date;
            return RetrieveWorkingSessions("select * from WorkingSession where Day = '"+date.ToString()+"'");
        }//works
        private List<WorkingSession> RetrieveWorkingSessions(string query)
        {
            Query = query;

            List<WorkingSession> workingSessions = new List<WorkingSession>();
            WorkingSession workingSession;

            using (connection = new Connection())
            using (SQLiteCommand command = new SQLiteCommand())
            {
                SQLiteConnection con = connection.Open(DataSource);
                command.Connection = con;
                command.CommandText = query;
                command.ExecuteNonQuery();
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    workingSession = new WorkingSession(Convert.ToInt32(reader[0]),
                         reader[1].ToString(), Convert.ToDateTime(reader[3].ToString()),
                         reader[2].ToString(), Convert.ToDateTime(reader[4].ToString()),
                         Convert.ToDateTime(reader[5].ToString()), Convert.ToBoolean(reader[6]));

                    workingSession.Tasks = tDao.RetrieveTasksForWorkingSession(workingSession);

                    workingSessions.Add(workingSession);
                }
            }

            return workingSessions;
        }//works
        public void SaveAsDoneWorkingSession(WorkingSession wrk)
        {
            wrk.Accomplished = true;
            Update(wrk);
        }//works

        /// <summary>
        /// Rerieves the last working session to be inserted in the database.
        /// </summary>
        /// <returns></returns>
        public WorkingSession RetrieveLastWorkingSession()
        {
            return RetrieveWorkingSession("SELECT * FROM "+Table+
                " WHERE   ID = (SELECT MAX(ID)  FROM "+Table+" );");
        }
        private WorkingSession RetrieveWorkingSession(string query)
        {
            WorkingSession workingSession;
            using (connection = new Connection())
            using (SQLiteCommand command = new SQLiteCommand())
            {
                SQLiteConnection con = connection.Open(DataSource);
                command.Connection = con;
                command.CommandText = query;
                command.ExecuteNonQuery();
                SQLiteDataReader reader = command.ExecuteReader();
                reader.Read();

                if(!reader.HasRows)
                {
                    throw new Exception("No Working sessions found in database");
                }
                
                workingSession = new WorkingSession(Convert.ToInt32(reader[0]),
                        reader[1].ToString(), Convert.ToDateTime(reader[3].ToString()),
                        reader[2].ToString(), Convert.ToDateTime(reader[4].ToString()),
                        Convert.ToDateTime(reader[5].ToString()), Convert.ToBoolean(reader[6]));

                workingSession.Tasks = tDao.RetrieveTasksForWorkingSession(workingSession);
            }
            return workingSession;
        }
        public int RetrieveAllWorkingSessionsCount()
        {
            return RetrieveCount("Select count() from " + Table);
        }//works
        public int RetrieveAccommplishedWorkingSessionsCount()
        {
            return RetrieveCount("select count() from WorkingSession where ID in"+
                " (select ID from WorkingSession where Accomplished = 'True')");
        }//works
        public int RetrieveNotAccommplishedWorkingSessionsCount()
        {
            return RetrieveCount("select count() from WorkingSession where ID in" +
                " (select ID from WorkingSession where Accomplished = 'False')");
        }//works
        public int RetrieveWorkingSessionsCountForDate(DateTime date)
        {
            return RetrieveCount("select count() from WorkingSession where Day = '" + date.ToString() + "'");
        }//works
        public void Update(WorkingSession wrk)
        {
            string condition = " Where ID = " + wrk.ID;
            Update("Name", wrk.Name, condition);
            Update("Description", wrk.Description, condition);
            Update("Day", wrk.Day.ToString(), condition);
            Update("StartTime", wrk.StartTime.ToString(), condition);
            Update("EndTime", wrk.EndTime.ToString(), condition);
            Update("Accomplished", wrk.Accomplished.ToString(), condition);
        }
        public WorkingSession RetrieveClosestWorkingSession()
        {
            return RetrieveWorkingSession("Select * from "+Table+" order by StartTime Desc Limit 1");
        }
    }
}
