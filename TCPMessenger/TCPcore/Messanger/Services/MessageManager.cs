using Messenger.Services.Messages;
using System.Collections.Generic;
using Seed;
using Seed.Shared;
using Dao.Entities;
using Dao;
using Messenger.Services.CheckConnection;
using Messenger.Services.CheckConnection.SubModules;
using System.Management;
using Messenger.Services.ChryptoGrapher;
using System.Collections.ObjectModel;
using System;
using System.Text;
using System.Diagnostics;

namespace Messenger.Services
{
    public class MessageManager
    {
        public const int PORT = 1909;

        ConnectionCheckerService conChecker;
        Client client;
        Server server;

        /// <summary>
        /// Fired When a new connected Artisan device is detected
        /// </summary>
        public event Action<ChatUser> ArtisanDetected;
        /// <summary>
        /// Fired when the connectivity on a wifi of the application changes
        /// </summary>
        public event Action<bool> ConnectivityChanged;
        /// <summary>
        /// All the sessions which have been connected
        /// Matched to their corresponding IPs this is to ease Communication.
        /// </summary>
        public Dictionary<ChatUser, Session> Sessions { get; private set; }

        public MessageManager()
        {
            Sessions = new Dictionary<ChatUser, Session>();

            ///Connection Checker section.
            conChecker = new ConnectionCheckerService(new WirelessNetCheckerService());
            conChecker.PingAllTerminated += OnPingAllTerminated;

            ///Global authentication object
            AuthenticationObject authObject;
            UserDao uDao = new UserDao("User");
            User user = uDao.retrieveUser();
            if(user != null)
            {
                authObject = new AuthenticationObject(user.Name, conChecker.NetChecker.GetIPAddress(), "");
            }
            else
            {
                authObject = new AuthenticationObject(user.Name, GetComputerName(), "");
            }

            //client Section.
            client = new Client(authObject);
            client.ConnectionEstablished += OnClientConnectionEstablished;
            client.ErrorOccured += OnClient_ErrorOccured;

            //Server session
            if(conChecker.NetChecker.IsConnectedToNetwork())
            {
                server = new Server(conChecker.NetChecker.GetIPAddress(), PORT, authObject);
            }
            else
            {
                server = new Server("127.0.0.1", PORT, authObject);
            }
            server.ConnectionAccepted += OnServer_ConnectionAccepted; ;
            server.ErrorOccured += OnServer_ErrorOccured;  
        }

        private void OnServer_ConnectionAccepted(Session session)
        {
            Debug.WriteLine("Server connection Accepted");

            session.AuthenticationObjectReceived += OnSessionAuthenticationObjectReceived;
        }

        private void OnServer_ErrorOccured(Exception ex, string IP, Seed.Enum.ErrorCode code)
        {
            Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
        }
        private void OnClient_ErrorOccured(Exception ex, string IP, Seed.Enum.ErrorCode code)
        {
            throw new NotImplementedException();
        }


        ///-----------------------------------------------------------------------------------------
        #region Detect every Artisan on the Network
        ///-----------------------------------------------------------------------------------------
        ///

        //This method starts the server and launches the detection of connected devices
        public void DetectArtisans()
        {
            server.Start();
            if (conChecker.NetChecker.IsConnectedToNetwork())
                conChecker.PingThemAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        private void OnClientConnectionEstablished(Session session)
        {
            Debug.WriteLine("client Connected Succesfully to "+session.IP);

            session.AuthenticationProtocol.Authenticate();
            session.AuthenticationObjectReceived += OnSessionAuthenticationObjectReceived;
        }

        private void OnSessionAuthenticationObjectReceived(AuthenticationObject obj, Session session)
        {
            Debug.WriteLine("Authentication from: "+obj.Name);

            ChatUser cUser = new ChatUser(obj.Name, obj.IP);
            Sessions.Add(cUser, session);
            ArtisanDetected?.Invoke(cUser);
        }

        private void OnPingAllTerminated(List<string> av, List<string> timeO)
        {
            foreach(string ip in av)
            {
                Debug.WriteLine(ip);

                client.Connect(ip, PORT);
            }
            client.Connect(conChecker.NetChecker.GetIPAddress(), PORT);
        }

        #endregion




        /// <summary>
        /// Retrieving Computer Name.
        /// </summary>
        /// <returns></returns>
        public static string GetComputerName()
        {
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            string info = string.Empty;
            foreach (ManagementObject mo in moc)
            {
                info = (string)mo["Name"];
                //mo.Properties["Name"].Value.ToString();
                //break;
            }
            return info;
        }
    }
}

