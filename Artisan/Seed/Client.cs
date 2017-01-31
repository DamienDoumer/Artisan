using System;
using System.Net.Sockets;
using Seed.Enum;
using Seed.Shared;
using System.Threading;
using System.Text;

namespace Seed
{
    public class Client
    {
        /// <summary>
        /// Fired when the client successfully connects to a server
        /// </summary>
        /// <param name="session"></param>
        public delegate void ConnectionEstablishedEventHandler(Session session);
        public event ConnectionEstablishedEventHandler ConnectionEstablished;

        /// <summary>
        /// This is fired when an error occures in the client
        /// </summary>
        /// <param name="error"></param>
        /// <param name="ex"></param>
        /// <param name="IP"></param>
        public delegate void ErrorOccuredEventHandler(Exception ex, string IP, ErrorCode code);
        public event ErrorOccuredEventHandler ErrorOccured;

        private Session session;
        private string iP;
        private int port;
        private TcpClient client;
        private bool connected;

        public bool Connected { get; }
        public SessionBase Session { get { return session; } }
        string IP { get { return iP; } }
        public int Port { get { return port; } }
        public TcpClient ClientCore { get { return client; } }
        public AuthenticationObject Authobject { get; private set; }


        public Client(AuthenticationObject obj)
        {
            client = new TcpClient();
            Authobject = obj;
        }

        /// <summary>
        /// Connect to a server on a different thread and create a session from there
        /// </summary>
        /// <param name="iP"></param>
        /// <param name="port"></param>
        public void Connect(string iP, int port)
        {
            Thread thread1 = new Thread(() => ConnectionProcess(iP, port));
            thread1.Start();
        }

        private void ConnectionProcess(string iP, int port)
        {
            this.iP = iP;
            this.port = port;
            int trial = 0;

            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.LingerState = new LingerOption(true, 10);
            serverSocket.NoDelay = true;
            client.Client = serverSocket;

            //tries to connect to the server 10 times before aborting
            while (trial < 10)
            {
                trial++;

                try
                {
                    serverSocket.Connect(IP, Port);

                    //this code below will only execute if the connection succeeds.
                    connected = true;
                    session = new Session(serverSocket, Authobject);
                    session.AuthObj = Authobject;
                    ConnectionEstablished(session);

                    break;
                }
                catch
                {
                    continue;
                }
            }

            if (connected == false)
            {
                ErrorOccured?.Invoke(new Exception("Connection to Server failed"), IP,
                        ErrorCode.ConnectionNotEstablished);
            }
        }
    }
}
