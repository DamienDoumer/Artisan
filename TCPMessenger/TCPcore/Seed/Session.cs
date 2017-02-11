using System;
using Seed.Shared;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Seed.Protocols;
using static System.Diagnostics.Debug;

namespace Seed
{
    public class Session : SessionBase, IDisposable
    {
        public event DataReceivedEventHanlder DataReceived;
        public event ErrorOccuredEventHandler ErrorOccured;
        public event AuthenticationObjectReceivedEventHanlder AuthenticationObjectReceived;
        public event SessionAbortedEventHandler SessionAborted;
        public event FileTransferInitiationEventHandler FileTransferInitiated;
        public event FileTransferProgressionEventHandler FileTransferProgression;
        public event FileTransferTerminatedEventHandler FileTransferTerminated;

        public Session(Socket socket, AuthenticationObject obj):base(socket, obj)
        {
            ///Set interface to have the authentication object
            ///For better code reuse
            AuthenticationProtocol = new Authentication(this, AuthObj);
        }

        /// <summary>
        /// Converts string to byte 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] ConvertStringToByte(string data)
        {
            return new byte[] { };
        }

        /// <summary>
        /// Converts data from byte to string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ConvertByteToString(byte[] data)
        {
            return string.Empty;
        }
        public override void Send(byte[] data)
        {
            //MemoryStream memStream = new MemoryStream();
            //BinaryWriter writer = new BinaryWriter(memStream, Encoding.UTF8);
            //writer.Write(data);
            //byte[] buffer = memStream.ToArray();
            SocketStream.Write(data, 0, data.Length);
            SocketStream.Flush();
        }

        public override void Start()
        {
            Thread thread = new Thread(new ThreadStart(() => ReadData()));
            thread.Start();
        }

        /// <summary>
        /// Reads data from the opened stream and notifies when the data has been read completely.
        /// </summary>
        private void ReadData()
        {
            //The memory stream helps in removing unwanted characters from data read.
            MemoryStream memStream = new MemoryStream();
            BinaryReader reader = new BinaryReader(SocketStream, Encoding.UTF8);
            AuthenticationObject obj = null;

            try
            {
                byte[] buffer = new byte[PacketSize];
                byte[] decrytedDataBytes = new byte[PacketSize];
                byte[] dataReceived = null;
                string dataString = null;

                int read = 0;

                lock (this)
                {
                    while (true)
                    {
                        read = reader.Read(buffer, 0, PacketSize);
                        memStream.Write(buffer, 0, read);
                        dataReceived = memStream.ToArray();

                        //decrypt the data received
                        //decrytedDataBytes = Cryptograph.Decrypt(buffer);

                        ///I set the stream to read after the previous message it read.
                        memStream.SetLength((memStream.Length - memStream.Position));
                        dataString = Encoding.UTF8.GetString(dataReceived);

                        //Refuse any form of communication if user is not authenticated is not established
                        if (!IsAuthtenticated)
                        {
                            //if the received message is an authentication request from the sender
                            if (dataString.Equals(AUTHENTICATION_REQUEST))
                            {
                                IsAuthtenticated = true;
                                obj = AuthenticationProtocol.ReceiveAuthenticationObject(memStream, reader);
                                AuthenticationObjectReceived?.Invoke(obj, this);

                                //If the sender is not already authnticated, Authenticate yourself 
                                if(obj.Authenticated == false)
                                {
                                    AuthObj.Authenticated = true;
                                    AuthenticationProtocol.Authenticate();
                                }
                            }
                            else
                            {
                                //if the correspondent is not authenticated, brutally seez the connection
                                SessionSocket.Close();
                            }
                        }
                        else
                        {
                            if(dataString.Equals(FILETRANSFER_REQUEST))
                            {
                                Document doc = ReceiveFileTransfer(memStream, reader);
                                FileTransferInitiated?.Invoke(doc);
                                ReceiveFile(doc, memStream, reader);
                            }
                            else
                            {
                                DataReceived?.Invoke(dataReceived, IP);
                            }
                        }

                        if (read == 0)
                        {
                            break;
                        }
                    }
                    SessionAborted?.Invoke(this);
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Unable to read data from the transport connection: An existing connection was forcibly closed by the remote host.")
                {
                    ErrorOccured?.Invoke(e, Enum.ErrorCode.ConnectionClosed, this);
                }
                else
                    if (e.Message == "Unable to read data from the transport connection: Cannot access a disposed object.\r\nObject name: 'System.Net.Sockets.Socket'..")
                {
                    ErrorOccured?.Invoke(e, Enum.ErrorCode.ConnectionClosed, this);
                }
                else
                {
                    SessionAborted?.Invoke(this);
                    throw e;
                }

            }
        }

        /// <summary>
        /// Send the file to the session
        /// </summary>
        /// <param name="doc"></param>
        public override void SendFile(Document doc)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    Send(Encoding.UTF8.GetBytes(FILETRANSFER_REQUEST));

                    string docString = Document.JsonSerialize(doc);
                    string jsonLength = docString.Length.ToString();
                    int finalLength = Convert.ToInt32(jsonLength.Length) + docString.Length;

                    //send the length of the serialized object + the object string
                    Send(Encoding.UTF8.GetBytes(finalLength + docString));

                    //FileStream fs = File.OpenRead(doc.DocPath);
                    //fs.CopyTo(SocketStream);
                    //fs.Close();

                    ///-------------------------------------------------------------------------
                    /// The process bellow ensures that every file byte is sent till the end
                    /// ------------------------------------------------------------------------
                    FileStream fileStream = new FileStream(doc.DocPath, FileMode.Open, FileAccess.Read);

                    int currentPacketSize = doc.PacketSize;
                    long totalPacketSize = doc.Size;
                    decimal percentage = 0;
                    long sentPackets = 0;
                    byte[] buffer = null;

                    for (int i = 0; i < doc.NumberOfPackets; i++)
                    {
                        if (totalPacketSize > doc.PacketSize)
                        {
                            currentPacketSize = doc.PacketSize;
                            totalPacketSize = totalPacketSize - currentPacketSize;
                        }
                        else
                        {
                            currentPacketSize = (int)totalPacketSize;
                        }

                        sentPackets += currentPacketSize;
                        percentage = Decimal.Divide((decimal)sentPackets, (decimal)doc.Size);
                        percentage = Decimal.Multiply(percentage, 100);
                        FileTransferProgression?.Invoke(percentage, IP);

                        WriteLine(percentage);

                        buffer = new byte[currentPacketSize];
                        fileStream.Read(buffer, 0, currentPacketSize);
                        SocketStream.Write(buffer, 0, buffer.Length);
                    }

                    fileStream.Close();

                    FileTransferTerminated?.Invoke(doc);
                }
                catch (Exception e)
                {
                    SessionAborted?.Invoke(this);
                    throw e;
                }
            });

            thread.Start();
        }

        /// <summary>
        /// Receives the Document object which represent's information about the file which
        /// is to be sent.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Document ReceiveFileTransfer(MemoryStream stream, BinaryReader reader)
        {
            byte[] buffer = new byte[PacketSize];
            byte[] dataReceived = null;
            int read = 0;
            string dataString = null;
            Document doc = null;

            try
            {
                lock (this)
                {
                    read = reader.Read(buffer, 0, PacketSize);
                    stream.Write(buffer, 0, read);
                    dataReceived = stream.ToArray();
                    stream.SetLength((stream.Length - stream.Position));
                    dataString = Encoding.UTF8.GetString(dataReceived);

                    dataString = RemoveUnwantedBytesFromJson(dataString);

                    WriteLine(dataString);

                    doc = Document.JsonDeserialize(dataString);
                }
            }
            catch (Exception e)
            {
                SessionAborted?.Invoke(this);
                throw e;
            }

            return doc;
        }
        /// <summary>
        /// receive the file coresponding to the document stated
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="stream"></param>
        /// <param name="reader"></param>
        private void ReceiveFile(Document doc, MemoryStream stream, BinaryReader reader)
        {
            ///-------------------------------------------------------------------------
            /// The proccess below ensures that every bit of file bytes sent is received
            /// ------------------------------------------------------------------------
            FileStream fStream = new FileStream(doc.Name, FileMode.OpenOrCreate, FileAccess.Write);

            try
            {
                int currentPacketSize = doc.PacketSize;
                decimal totalPacketSize = doc.Size;
                decimal percentage = 0;
                decimal sentPackets = 0;
                byte[] buffer = new byte[PacketSize];
                byte[] dataReceived = null;
                int read = 0;

                for (int i = 0; i < doc.NumberOfPackets; i++)
                {
                    if (totalPacketSize > doc.PacketSize)
                    {
                        currentPacketSize = doc.PacketSize;
                        totalPacketSize = totalPacketSize - currentPacketSize;
                    }
                    else
                    {
                        currentPacketSize = (int)totalPacketSize;
                    }

                    sentPackets += currentPacketSize;
                    read = reader.Read(buffer, 0, PacketSize);

                    stream.Write(buffer, 0, read);
                    percentage = Decimal.Divide((decimal)sentPackets, (decimal)doc.Size);
                    percentage = Decimal.Multiply(percentage, 100);

                    dataReceived = stream.ToArray();
                    stream.SetLength((stream.Length - stream.Position));
                    stream.Flush();
                    FileTransferProgression?.Invoke(percentage, IP);

                    fStream.Write(dataReceived, 0, read);
                }

                fStream.Close();
            }
            catch (Exception e)
            {
                SessionAborted?.Invoke(this);
                throw e;
            }
        }

        /// <summary>
        /// Returns the json object without the unwanted characters
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private string RemoveUnwantedBytesFromJson(string json)
        {
            int firstBracket = json.IndexOf('{');
            StringBuilder objectBuilder = new StringBuilder();
            int objL = 0;

            for(int i = 0;i<firstBracket; i++)
            {
                objectBuilder.Append(json[i]);
            }

            objL = Convert.ToInt32(objectBuilder.ToString());

            objectBuilder.Clear();

            for (int i = firstBracket; i < objL; i++)
                {
                    objectBuilder.Append(json[i]);
                }

            return objectBuilder.ToString();
        }
    }
}
