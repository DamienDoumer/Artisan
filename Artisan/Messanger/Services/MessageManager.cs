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

namespace Messenger.Services
{
    public class MessageManager
    {
        private ConnectionCheckerService conChecker;
        private Server server;
        private Client client;
        private int port;
        private UserDao userDao;
        private User user;

        public delegate void ScanArtisanStartedEventHandler();
        public event ScanArtisanStartedEventHandler ScanArtisanStarted;
        public delegate void ScanArtisanTerminatedEventHandler(List<ChatUser> users);
        public event ScanArtisanTerminatedEventHandler ScanArtisanTerminated;

        /// <summary>
        /// This object corresponds to the IP address of each session, 
        /// And its related Messages, While precising iif the user sent or received the message
        /// </summary>
        public static Dictionary<ChatUser, ObservableCollection<SimpleMessage>> Conversations { get; private set; }
        public Session CurrentSession { get; private set; }
        /// <summary>
        /// This corresponds tho the users who are connected
        /// </summary>
        public ObservableCollection<ChatUser> ConnectedUsers { get; private set; }
        /// <summary>
        /// List of user sessions.
        /// </summary>
        public List<Session> Sessions { get; set; }

        public MessageManager()
        {
            port = 1909;
            Conversations = new Dictionary<ChatUser, ObservableCollection<SimpleMessage>>();
            ConnectedUsers = new ObservableCollection<ChatUser>();
            Sessions = new List<Session>();
            conChecker = new ConnectionCheckerService(new WirelessNetCheckerService(), new object());
            userDao = new UserDao("User");
            user = new User("", "", "", "");
            user = userDao.retrieveUser();

            ///If the user has not yet saves his credentials, get the computer's name
            if(user == null)
            {
                user.Name = GetComputerName();
            }

            server = new Server(conChecker.NetChecker.GetIPAddress(), port,
                new AuthenticationObject(user.Name, conChecker.NetChecker.GetIPAddress(), ""), new MD5CryptographicService());

            client = new Client(new AuthenticationObject(user.Name, conChecker.NetChecker.GetIPAddress(), ""), new MD5CryptographicService());

            ///Server code
            server.ConnectionAccepted += OnConnectionEstablished;
            server.ErrorOccured += OnServer_ErrorOccured;

            ///client code
            client.ConnectionEstablished += OnConnectionEstablished;

            ///Connection checker handling code
            conChecker.PingAllTerminated += OnConChecker_PingAllTerminated;
        }

        private void OnServer_ErrorOccured(Exception ex, string IP, Seed.Enum.ErrorCode code)
        {
            throw new NotImplementedException();
        }
        
        private void OnConnectionEstablished(Session session)
        {
            session.AuthenticationObjectReceived += OnSession_AuthenticationObjectReceived;
            session.ErrorOccured += OnSession_ErrorOccured;
            session.FileTransferInitiated += OnSession_FileTransferInitiated;
            session.FileTransferProgression += OnSession_FileTransferProgression;
            session.FileTransferTerminated += OnSession_FileTransferTerminated;
            session.MessageReceived += OnSession_MessageReceived;

            session.AuthenticationProtocol.Authenticate();

            Sessions.Add(session);
        }

        /// <summary>
        /// Add received message to the corresponding conversations.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="iP"></param>
        private void OnSession_MessageReceived(string message, string iP)
        {
            foreach(ChatUser usr in ConnectedUsers)
            {
                if(usr.IP == iP)
                {
                    Conversations[usr].Add(new SimpleMessage(message, DateTime.Now, false));
                }
            }
        }

        private void OnSession_FileTransferTerminated(Document doc)
        {
            throw new System.NotImplementedException();
        }

        private void OnSession_FileTransferProgression(decimal percentage, string ip)
        {
            throw new System.NotImplementedException();
        }

        private void OnSession_FileTransferInitiated(Document doc)
        {
            throw new System.NotImplementedException();
        }

        private void OnSession_ErrorOccured(Exception ex, Seed.Enum.ErrorCode error, SessionBase session)
        {
            foreach(ChatUser usr in ConnectedUsers)
            {
                if (session.IP == usr.IP)
                {
                    Conversations.Remove(usr);
                    ConnectedUsers.Remove(usr);
                    break;
                }
            }

            Sessions.Remove(session as Session);

            try
            {
                session.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// when the user suthenticates
        /// add the user to the list of connected people 
        /// and add the user to the conversations
        /// </summary>
        /// <param name="obj"></param>
        private void OnSession_AuthenticationObjectReceived(AuthenticationObject obj)
        {
            ///When authentication completes, save user as connected user.
            ConnectedUsers.Add(new ChatUser(obj.Name, obj.IP));
            Conversations.Add(new ChatUser(obj.Name, obj.IP), new ObservableCollection<SimpleMessage>());
        }

        /// <summary>
        /// Start the complex processes emiditately after constuction.
        /// </summary>
        public void StartProcesses()
        {
            server.Start();
        }

        /// <summary>
        /// Fired when the ping process finishes
        /// </summary>
        /// <param name="av"></param>
        /// <param name="timeO"></param>
        private void OnConChecker_PingAllTerminated(List<string> av, List<string> timeO)
        {
            foreach(string ip in av)
            {
                client.Connect(ip, port);
            }
        }

        /// <summary>
        /// Scans the network to find if there are artisan 
        /// users on the network.
        /// </summary>
        public void ScanForArtisans()
        {
            ScanArtisanStarted?.Invoke();
            conChecker.PingThemAll();
        }
        public void SendMessage(ChatUser user, SimpleMessage message)
        {
            foreach(Session ses in Sessions)
            {
                if(ses.IP == user.IP)
                {
                    ses.Send(Encoding.UTF8.GetBytes(message.Text));
                }
            }

            Conversations[user].Add(message);
        }

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
