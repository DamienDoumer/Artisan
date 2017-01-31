using System;
using System.Collections.Generic;
using System.Threading;
using Seed.Enum;
using System.Net.Sockets;
using System.Net;
using Seed.Shared;

namespace Seed
{
    public class Server
    {
        public AuthenticationObject Authobject { get; private set; }

        /// <summary>
        /// This is fired when the server accepts a new connection.
        /// </summary>
        /// <param name="session"></param>
        public delegate void ConnectionAcceptedEventHanlder(Session session);
        public event ConnectionAcceptedEventHanlder ConnectionAccepted;

        /// <summary>
        /// This is fired when a specific error I want to handle occures not any exception in the client
        /// </summary>
        /// <param name="error"></param>
        /// <param name="ex"></param>
        /// <param name="IP"></param>
        public delegate void ErrorOccuredEventHandler(Exception ex, string IP, ErrorCode code);
        public event ErrorOccuredEventHandler ErrorOccured;

        private TcpListener listener;
        public string IP { get; set; }
        public int Port { get; set; }
        public List<Session> Sessions;
        public TcpListener Listener { get { return listener; } }

        public Server(string iP, int port, AuthenticationObject obj)
        {
            Authobject = obj;
            IP = iP;
            Port = port;
            Sessions = new List<Session>();
        }

        public void Start()
        {
            try
            {
                listener = new TcpListener(IPAddress.Parse(IP), Port);

                Thread thread1 = new Thread
                (
                    new ThreadStart(() => Listen(listener))
                );
                thread1.Start();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void Listen(TcpListener listener)
        {
            Socket clientSocket = null;
            
            try
            {
                listener.Start();

                while (true)
                {
                    clientSocket = listener.AcceptSocket();

                    lock(this)
                    {
                        if(clientSocket.Connected)
                        {
                            Session session = new Session(clientSocket, Authobject);
                            Sessions.Add(session);
                            ConnectionAccepted?.Invoke(session);
                        }
                    }
                }
            }
            catch(Exceptio e)
            {
                //this exception is thrown when the port is busy
                if(e.Message == "An attempt was made to access a socket in a way forbidden by its access permissions")
                {
                    ErrorOccured?.Invoke(e, IP, ErrorCode.PortBusy);
                }
                else
                    if(e.Message == "Only one usage of each socket address (protocol/network address/port) is normally permitted")
                {
                    ErrorOccured.Invoke(e, IP, ErrorCode.ServerSocketInUse);
                }
                else
                {
                    throw e;
                }
            }
        }
    }
}
