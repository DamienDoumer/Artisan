using System;
using System.Collections.Generic;
using System.Text;
using Dao.Entities;
using System.Data.SQLite;

namespace Dao
{
    public class TaskDao : BaseDao
    {
        public static string CreateStatement { get; } = "CREATE TABLE `Task` ( " +
                       " `ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        "`Name`	VarChar(50) NOT NULL," +
                        " `Accomplished`	boolean, "+
                        " `WorkingSessionID`	INTEGER, FOREIGN KEY(`WorkingSessionID`) REFERENCES WorkingSession(ID) " +
                        ")";

        public TaskDao(string table) :base(table)
        {}

        public bool DeleteAllTasks()
        {
            return base.DeleteAll(CreateStatement);
        }//Works
        public bool Delete(Task task)
        {
            return base.Delete(task.ID);
        }//Works
        public bool Save(Task data)
        {
            string attribs = CreateStringAttribute(data.GetProperties());
            string attribVals = CreateStringAttributeValues(data.GetPropertyValues());
            StringBuilder build = new StringBuilder();
            int success = 0;

            build.Append("Insert into ").Append(Table)
                        .Append(" " + attribs).Append(" Values " + attribVals);

            success = base.ExecuteQuery(build.ToString());

            if (success == 1)
                return true;
            return false;
        }//Works
        public List<Task> RetrieveAllTasks()
        {
            Table = "Task";
            return RetrieveTasks("Select * from " + Table);
        }//Works
        public List<Task> RetrieveTasksForWorkingSession(WorkingSession wrk)
        {
            Table = "Task";
            return RetrieveTasks("Select * from Task where WorkingSessionID = " + wrk.ID);
        }//Works
        private List<Task> RetrieveTasks(string query)
        {
            Table = "Task";
            List<Task> tasks = new List<Task>();
            Task task;
            Query = query;

            using (connection = new Connection())
                using (SQLiteCommand command = new SQLiteCommand())
            {
                SQLiteConnection con = connection.Open(DataSource);
                command.Connection = con;
                command.CommandText = Query;
                command.ExecuteNonQuery();
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    task = new Task(Convert.ToInt32(reader[0]), reader[1].ToString(), 
                        Convert.ToInt32(reader[2]), Convert.ToBoolean(reader[3]));
                    tasks.Add(task);
                }
                reader.Close();
            }

            return tasks;
        }//Works
        public bool DeleteTasksForWorkingSession(WorkingSession wrk)
        {
            Table = "Task";
            Query = "Delete from "+Table+" where WorkingSessionID = "+wrk.ID;
            Console.WriteLine(Query);
            if (ExecuteQuery(Query) == 1)
                return false;
            return true;
        }//works
        public List<Task> RetrieveAccomplishedTasksForWorkingSession(WorkingSession wrk)
        {
            return RetrieveTasks("select * from task where Accomplished = 'true' and WorkingSessionID = "+wrk.ID);
        }//works
        public List<Task> RetrieveUnAccomplishedTasksForWorkingSession(WorkingSession wrk)
        {
            return RetrieveTasks("select * from task where Accomplished = 'true' and WorkingSessionID = " + wrk.ID);
        }//works
        public List<Task> RetrieveAccomplishedTasks()
        {
            return RetrieveTasks("select * from task where Accomplished = 'true'");
        }
        public List<Task> RetrieveUnAccomplishedTasks()
        {
            return RetrieveTasks("select * from task where Accomplished = 'false'");
        }
        public int RetrieveAccomplishedTasksCount()
        {
            return RetrieveCount("select count() from task where Accomplished = 'true'");
        }
        public int RetrieveUnAccomplishedTasksCount()
        {
            return RetrieveCount("select count() from task where Accomplished = 'false'");
        }
        public int RetrieveAccomplishedTasksCountForWorkingSession(WorkingSession wrk)
        {
            return RetrieveCount("select count() from task where Accomplished = 'true' and WorkingSessionID = " + wrk.ID);
        }//Works
        public int RetrieveUnAccomplishedTasksCountForWorkingSession(WorkingSession wrk)
        {
            return RetrieveCount("select count() from task where Accomplished = 'true' and WorkingSessionID = " + wrk.ID);
        }//works
        public int RetrieveAllTasksCount()
        {
            return RetrieveCount("Select count() from " + Table);
        }//works
        public void Update(Task task)
        {
            Update("Name", task.Name, "Where ID = " + task.ID);
        }
    }
}
