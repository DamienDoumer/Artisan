using System.IO;
using System.Net.Sockets;
using System;
using Seed.Enum;
using Seed.Cryptography;
using System.Text;
using Seed.Protocols;

namespace Seed.Shared
{
    public class SessionBase : IDisposable
    {
        public const string AUTHENTICATION_REQUEST = ">!AUTH!<";
        public const string FILETRANSFER_REQUEST = ">!FILE!<";
        public const string CHAT_REQUEST = "";
        public const string STOP = ">!STOP!<";

        /// <summary>
        /// This is in charge of encrypting and decrypting every message sent or received
        /// </summary>
        public ICryptographer Cryptograph { get; set; }

        /// <summary>
        /// Notifies the user that a file is being transfered.
        /// </summary>
        /// <param name="doc">Contains information about the file being transfered</param>
        /// <param name="ip">IP of the person transfering</param>
        public delegate void FileTransferInitiationEventHandler(Document doc);
        /// <summary>
        /// Notifies that the transfer of the file is over 
        /// It contains even the IP address of the sender
        /// </summary>
        /// <param name="doc"></param>
        public delegate void FileTransferTerminatedEventHandler(Document doc);
        /// <summary>
        /// Notifies the user on how the transfer is progressing
        /// </summary>
        /// <param name="percentage"></param>
        /// <param name="ip"></param>
        public delegate void FileTransferProgressionEventHandler(decimal percentage, string ip);
        /// <summary>
        /// Fired when the session is suddenly disposed or aborted
        /// </summary>
        /// <param name="session"></param>
        public delegate void SessionAbortedEventHandler(SessionBase session);
        /// <summary>
        /// Fired when this session receives data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="iP"></param>
        public delegate void DataReceivedEventHanlder(byte[] data, string iP);
        /// <summary>
        /// Fired when a known and handled error occures in this session
        /// </summary>
        /// <param name="error"></param>
        public delegate void ErrorOccuredEventHandler(Exception ex, ErrorCode error, SessionBase session);
        /// <summary>
        /// Thrown when the end host wants to authenticate
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="stream"></param>
        public delegate void AuthenticationObjectReceivedEventHanlder(AuthenticationObject obj);
        /// <summary>
        /// Fired when the end user is initiating a chat
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="session"></param>
        public delegate void ChatRequestReceivedEventHandler(string ip, SessionBase session);
        /// <summary>
        /// Fired when the end user sends his user's credentials
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="session"></param>
        public delegate void IdentificationRequestReceivedEventHandler(string ip, SessionBase session);

        public Socket SessionSocket { get; protected set; }
        public string IP { get; set; }
        public bool IsAuthtenticated { get; protected set; }
        public int PacketSize { get; set; }
        public NetworkStream SocketStream { get; protected set; }
        /// <summary>
        /// Used in authenticating which object is on the network and is connected
        /// this object contains the user's data
        /// </summary>
        public AuthenticationObject AuthObj { get; set; }

        /// <summary>
        /// this is used to implement the authentication protocol in the session.
        /// </summary>
        public IAuthentication AuthenticationProtocol { get; set; }

        public SessionBase(Socket socket, AuthenticationObject obj)
        {
            PacketSize = 2048;
            SessionSocket = socket;
            SessionSocket.LingerState = new LingerOption( true, 10);
            SessionSocket.NoDelay = true;
            SessionSocket.ReceiveBufferSize = PacketSize;
            SessionSocket.SendBufferSize = PacketSize;
            IP = GetIP(socket.RemoteEndPoint.ToString());
            IsAuthtenticated = false;
            SocketStream = new NetworkStream(socket);
            Cryptograph = new MD5Cryptographer();
            AuthObj = obj;
        }

        /// <summary>
        /// Sends  a packet of data to the stream
        /// </summary>
        /// <param name="data">data packet</param>
        public virtual void Send(byte[] data)
        {

        }

        /// <summary>
        /// Starts the reading process on the stream
        /// </summary>
        public virtual void Start()
        {

        }
        
        /// <summary>
        /// Removes the port number from the socket's IP address
        /// </summary>
        /// <param name="ip"></param>
        /// <returns>clean IP without any port</returns>
        private string GetIP(string ip)
        {
            StringBuilder build = new StringBuilder();
            int i = 0;
            while (ip[i] != ':')
            {
                build.Append(ip[i]);
                i++;
            }
            return string.Concat(build);
        }

        public override bool Equals(object obj)
        {
            SessionBase session = obj as SessionBase;
            return this.IP == session.IP;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public virtual void SendFile(Document doc)
        {
            
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SessionSocket.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SessionBase() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
