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

        #region Singleton region

        private static MessageManager instance;
        public static MessageManager Instance { get { return instance; }
            set { instance = value; } }

        static MessageManager()
        {
            instance = new MessageManager();
        }

        private MessageManager()
        {

            Sessions = new Dictionary<ChatUser, Session>();
            ConnectedUsers = new List<ChatUser>();

            ///Connection Checker section.
            conChecker = new ConnectionCheckerService(new WirelessNetCheckerService());
            conChecker.PingAllTerminated += OnPingAllTerminated;
            conChecker.NetChecker.IPAddressChanged += OnNetChecker_IPAddressChanged;
            conChecker.NetChecker.NetworkConnectivityChanged += OnNetChecker_NetworkConnectivityChanged;

            UserDao uDao = new UserDao("User");
            User user = uDao.retrieveUser();
            if (user != null)
            {
                authObject = new AuthenticationObject(user.Name, conChecker.NetChecker.GetIPAddress(), "");
            }
            else
            {
                authObject = new AuthenticationObject(GetComputerName(), conChecker.NetChecker.GetIPAddress(), "");
            }

            //client Section.
            client = new Client(authObject);
            client.ConnectionEstablished += OnClientConnectionEstablished;
            client.ErrorOccured += OnClient_ErrorOccured;

            //Server session
            if (conChecker.NetChecker.IsConnectedToNetwork())
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

        #endregion

        public const int PORT = 1909;

        ///Global authentication object
        AuthenticationObject authObject;
        ConnectionCheckerService conChecker;
        Client client;
        Server server;

        /// <summary>
        /// THis is fired when all the chats, conected usrs and all the like should
        /// be reset, for example when connection or IP changes.
        /// </summary>
        public event Action ResetAll;
        /// <summary>
        /// Fired when the user receives a message
        /// </summary>
        public event Action<ChatUser, SimpleMessage> MessageReceived;
        /// <summary>
        /// Fired When a new connected Artisan device is detected
        /// </summary>
        public event Action<ChatUser> ArtisanDetected;
        /// <summary>
        /// Fired when the connectivity on a wifi of the application changes
        /// </summary>
        public event Action<bool> ConnectivityChanged;
        /// <summary>
        /// Fired when an artisan is disconnected from teh network
        /// </summary>
        public event Action<ChatUser> ArtisanDisconnected;
        
        public List<ChatUser> ConnectedUsers { get; }
        /// <summary>
        /// All the sessions which have been connected
        /// Matched to their corresponding IPs this is to ease Communication.
        /// </summary>
        public Dictionary<ChatUser, Session> Sessions { get; private set; }


     
        public void SendMessage(ChatUser user, string message)
        {
            ///Encrypt data before sending it!!!!!!!!!!!!!!

            string msg = MD5CryptographicService.Encrypt(message);

            Sessions[user].Send(Encoding.UTF8.GetBytes(msg));
        }

        private void OnNetChecker_NetworkConnectivityChanged(System.Net.NetworkInformation.NetworkInterface netInterface, bool isConnected, string message)
        {
            //if(conChecker.NetChecker.IsConnectedToNetwork())
            //{
            //    ConnectivityChanged?.Invoke(true);
            //    RestartServer(conChecker.NetChecker.GetIPAddress());
            //}
            //else
            //{
            //    ConnectivityChanged?.Invoke(false);
            //    server.Stop();
            //    server = null;
            //}
        }

        private void OnNetChecker_IPAddressChanged(string IP)
        {
            //ResetAll?.Invoke();
            //Sessions.Clear();
            //ConnectedUsers.Clear();
            //RestartServer(IP);
        }

        private void RestartServer(string IP)
        {
            server.Stop();
            server = new Server(IP, PORT, authObject);
            DetectArtisans();
        }

        private void OnServer_ConnectionAccepted(Session session)
        {
            Debug.WriteLine("-----------------------------");
            Debug.WriteLine("Server connection Accepted: "+session.IP);
            Debug.WriteLine("-----------------------------");

            session.Start();

            session.AuthenticationObjectReceived += OnSessionAuthenticationObjectReceived;
            session.DataReceived += OnSession_DataReceived;
            session.ErrorOccured += OnSession_ErrorOccured;
        }

        private void OnSession_ErrorOccured(Exception ex, Seed.Enum.ErrorCode error, SessionBase session)
        {
            ConnectedUsers.Remove(new ChatUser("", session.IP));
            Sessions.Remove(new ChatUser("", session.IP));

            ArtisanDisconnected?.Invoke(new ChatUser("", session.IP));

            session.Dispose();

            Debug.WriteLine("-----------------------------");
            Debug.WriteLine("Session error ocured: " + ex.Message);
            Debug.WriteLine("-----------------------------");
        }

        private void OnServer_ErrorOccured(Exception ex, string IP, Seed.Enum.ErrorCode code)
        {
            Debug.WriteLine("---------------------------------");
            Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
            Debug.WriteLine("---------------------------------");
        }
        private void OnClient_ErrorOccured(Exception ex, string IP, Seed.Enum.ErrorCode code)
        {
            Debug.WriteLine("-----------------------------");
            Debug.WriteLine("Client error occuredd: " + ex.Message);
            Debug.WriteLine("------------------------------");
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
            Debug.WriteLine("-----------------------------");
            Debug.WriteLine("client Connected Succesfully to "+session.IP);
            Debug.WriteLine("-----------------------------");

            session.Start();

            session.AuthenticationProtocol.Authenticate();

            session.AuthenticationObjectReceived += OnSessionAuthenticationObjectReceived;
            session.DataReceived += OnSession_DataReceived;
            session.ErrorOccured += OnSession_ErrorOccured;
        }

        private void OnSession_DataReceived(byte[] data, string iP)
        {
            string message = MD5CryptographicService.Decrypt(Encoding.UTF8.GetString(data));

            foreach(ChatUser usr in ConnectedUsers)
            {
                if(usr.IP == iP)
                {
                    MessageReceived?.Invoke(usr, new SimpleMessage(message, DateTime.Now, false));
                }
            }

            Debug.WriteLine("-----------------------------");
            Debug.WriteLine("Data Received: "+message);
            Debug.WriteLine("-----------------------------");
        }

        private void OnSessionAuthenticationObjectReceived(AuthenticationObject obj, Session session)
        {
            Debug.WriteLine("-----------------------------");
            Debug.WriteLine("Authentication from: "+obj.Name);
            Debug.WriteLine("-----------------------------");

            ChatUser cUser = new ChatUser(obj.Name, obj.IP);

            if (!Sessions.ContainsKey(cUser))
            {
                ConnectedUsers.Add(cUser);
                Sessions.Add(cUser, session);
                ArtisanDetected?.Invoke(cUser);
            }
        }

        private void OnPingAllTerminated(List<string> av, List<string> timeO)
        {
            foreach(string ip in av)
            {
                Debug.WriteLine("-----------------------------");
                Debug.WriteLine("Ping all teminated: "+ip);
                Debug.WriteLine("-----------------------------");

                client.Connect(ip, PORT);
            }
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

