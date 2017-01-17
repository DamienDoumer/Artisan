using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using Dao.ShareResources;
using System.IO;

namespace Dao
{
    public class BaseDao
    {
        protected Connection connection;

        /// <summary>
        /// Fired when the database was not found and there is need to create a new database
        /// </summary>
        public delegate void DataBaseNotFoundEventHandler(string path, BaseDao dao);
        public event DataBaseNotFoundEventHandler DatabaseNotFound;
        
        public Connection Conn { get { return connection; } }
        public string DataBase { get; set; }
        public string Table { get; set; }
        public string Query { get; set; }
        public string DataSource { get; private set; }
        /// <summary>
        /// Specifies if this is the first time the app is used
        /// </summary>
        public bool FirstUse { get; set; }

        public BaseDao(string table)
        {
            ///Checks if the database exists in the precise if it doesnt, it creates one.
            if(File.Exists(Shared.DataBaseName))
            {
                DataBase = Shared.DataBaseName;
                Table = table;
                DataSource = "Data Source = " + DataBase + "; version = 3;";
            }
           else
            {
                ///Sets the first use attribute to truem specifying that the user has never used the app
                FirstUse = true;
                CreateDatabaseAndDirectory();
            }
        }

        protected virtual bool Save(object data)
        {
            return true;
        }
        protected virtual object Retrieve(object obj)
        {
            return string.Empty;
        }
        protected virtual object RetrieveByPK(int pk)
        {
            return null;
        }

        protected virtual List<object> RetrieveAll()
        {
            return null; 
        }

        protected virtual object Retrieve(string attrib, string atribValue)
        {
            return null;
        }
        protected int RetrieveCount(string query)
        {
            int count = 0;
            Query = query;

            using (connection = new Connection())
            using (SQLiteCommand command = new SQLiteCommand())
            {
                SQLiteConnection con = connection.Open(DataSource);
                command.Connection = con;
                command.CommandText = query;
                command.ExecuteNonQuery();
                SQLiteDataReader reader = command.ExecuteReader();

                reader.Read();
                count = Convert.ToInt32(reader[0].ToString());

                reader.Close();
            }
            return count;
        }//works

        protected virtual bool DeleteAll(string createStatement)
        {
            Query = "Drop Table " + Table;

            ExecuteQuery(Query);

            if(ExecuteQuery(createStatement)==1)
            {
                return false;
            }
            return true;
        }//Works

        public virtual bool Delete(int id)
        {
            Query = "delete from "+Table+" where id = "+id;

            if(ExecuteQuery(Query) == 1)
            {
                return true;
            }
            return false;
        }//Works
        protected string CreateStringAttributeValues(string[] val)
        {
            StringBuilder build = new StringBuilder();
            int v = 0;
            string start = "( ";
            string end = "); ";

            build.Append(start);
            if (int.TryParse(val[0], out v))//test if the value is an int
            {
                build.Append(val[0] + ", ");
            }
            else
            {
                build.Append("'" + val[0] + "', ");
            }

            for (int i = 1; i < val.Length; i++)
            {
                if (int.TryParse(val[i], out v)) //test if value is an integer
                {
                    if (i == val.Length - 1)
                    {
                        build.Append(val[i]);
                    }
                    else
                    {
                        build.Append(val[i] + ", ");
                    }
                }
                else
                {
                    if (i == val.Length - 1)
                    {
                        build.Append("'" + val[i] + "'");
                    }
                    else
                    {
                        build.Append("'" + val[i] + "', ");
                    }
                }
            }

            build.Append(end);
            return Convert.ToString(build);
        }//Works
        protected string CreateStringAttribute(string[] val)
        {
            StringBuilder builder = new StringBuilder();
            string start = "( ";
            string end = ") ";

            builder.Append(start);
            for (int i = 0; i < val.Length; i++)
            {
                if (i == val.Length - 1)
                {
                    builder.Append(val[i]);
                }
                else
                {
                    builder.Append(val[i] + ", ");
                }
            }

            builder.Append(end);
            return builder + "";
        }//Works
        protected int ExecuteQuery(string query)
        {
            int success = 0;
            Query = query;

            using (connection = new Connection())
                using (SQLiteCommand command = new SQLiteCommand())
            {
                SQLiteConnection con = connection.Open(DataSource);
                command.Connection = con;
                command.CommandText = query;
                success = command.ExecuteNonQuery();
            }
            return success;
        }//Works
        protected bool Update(string attrib, string attribVal, string conditions = null)
        {
            Query = null;
            if(conditions ==null)
            {
                Query = "Update " + Table + " Set " + attrib + " = '" + attribVal + "' ";
            }
            else
            {
                Query = "Update " + Table + " Set " + attrib + " = '" + attribVal + "' "+conditions;
            }
            int success = ExecuteQuery(Query);

            if (success == 1)
                return true;
            return false;
        }//Works

        /// <summary>
        /// this creates directory where files received via wifi communicatino are stored
        /// </summary>
        public void CreateDatabaseAndDirectory()
        {
            try
            {
                Directory.CreateDirectory(Shared.DataBasePath);
                DataBase = Shared.DataBasePath + "\\PerfDB.db";
                DataSource = "Data Source = " + DataBase + "; version = 3;";
                int success = 0;

                using (connection = new Connection())
                {
                    using (SQLiteCommand command = new SQLiteCommand())
                    {
                        command.Connection = connection.CreateDatabase(DataBase, DataSource);
                        command.CommandText = WorkingSessionDao.CreateStatement;
                        success = command.ExecuteNonQuery();
                        command.CommandText = EventDao.CreateStatement;
                        success = command.ExecuteNonQuery();
                        command.CommandText = TaskDao.CreateStatement;
                        success = command.ExecuteNonQuery();
                        command.CommandText = SettingsDao.CreateStatement;
                        success = command.ExecuteNonQuery();
                        command.CommandText = UserDao.CreateStatement;
                        success = command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE \"DaysOfWork\" ( " +
                                              " `Day`	varchar(20), " +
                                              " `TimeSpentOnPC`	INTEGER, " +
                                               "  PRIMARY KEY(Day) " +
                                               " );";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE `DoneWorkingSession` ( " +
                                              "`WorkingSessionID`	INTEGER NOT NULL, " +
                                              " `ComplitionDateTime`	varchar(30) NOT NULL, " +
                                              " FOREIGN KEY(`WorkingSessionID`) REFERENCES WorkingSession(ID) )";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE `Monitor` ( " +
                                               " `SecondsSpentOnPC`	INTEGER NOT NULL, " +
                                               " `Day`	VarChar(30), " +
                                               " PRIMARY KEY(Day) " +
                                                " )";
                        command.ExecuteNonQuery();
                        
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
