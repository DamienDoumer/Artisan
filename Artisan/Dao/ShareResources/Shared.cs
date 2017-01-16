using System;

namespace Dao.ShareResources
{
    /// <summary>
    /// Contains important data about the app like settings data
    /// </summary>
    public class Shared
    {
        private static string databasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DMera\\Data";
        private static string receptionFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>
        /// This is the directory where files received from transfer are stored
        /// </summary>
        public static string ReceptionFilePath { get {return receptionFilePath; }}
        public static string DataBasePath { get { return databasePath; } }
        public static string DataBaseName
        {
            get
            { return DataBasePath+"\\PerfDB.db"; }
        }
        public static string WorkingSessionTableName
        {
            get { return "WorkingSession"; }
        }
        public static string TaskTableName
        {
            get { return "Task"; }
        }
    }
}
