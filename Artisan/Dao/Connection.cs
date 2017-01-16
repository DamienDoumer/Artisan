using System;
using System.Data.SQLite;

namespace Dao
{
    public class Connection : IDisposable
    {
        private SQLiteConnection connection;
        public SQLiteConnection Con { get { return connection; } }
        
        public Connection()
        {
            connection = new SQLiteConnection();
        }

        public SQLiteConnection Open(string database)
        {
            try
            {
                connection.ConnectionString = database;
                connection.Open();
            }
            catch (Exception e)
            {
                throw e;
            }

            return connection;
        }

        /// <summary>
        /// Creates the database file in teh precised directory and returns an opened connection object to that directory
        /// </summary>
        /// <param name="path">Path to were the database is stored</param>
        /// <param name="connectionString">string used to access database.</param>
        /// <returns>Opened SQLiteConnection object</returns>
        public SQLiteConnection CreateDatabase(string path, string connectionString)
        {
            try
            {
                SQLiteConnection.CreateFile(path);
                connection.ConnectionString = connectionString;
                connection.Open();
            }
            catch (Exception e)
            {
                throw e;
            }

            return connection;
        }

        public void Close()
        {
            try
            {
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
