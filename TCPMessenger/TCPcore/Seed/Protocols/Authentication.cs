using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Seed.Shared;
using System.IO;
using Seed.Delegates;

namespace Seed.Protocols
{
    public class Authentication : IAuthentication
    {
        public Session AuthSession { get; set; }
        AuthenticationObject AuthObject { get; }
        public const string STOPAUTHENTICATION = ">!STOPAUTH!<";
        
        public Authentication(SessionBase session, AuthenticationObject obj)
        {
            AuthSession = session as Session;
            AuthObject = obj;
        }

        public void Authenticate()
        {
            AuthSession.Send(Encoding.UTF8.GetBytes(SessionBase.AUTHENTICATION_REQUEST));

            string json = AuthenticationObject.GetJsonString(AuthObject);
            AuthSession.Send(Encoding.UTF8.GetBytes(json));

            ////read the picture and send it to the stream
            //byte[] buffer = new byte[AuthSession.PacketSize];
            //FileStream fs = new FileStream(AuthObject.PicturePath, FileMode.Open, FileAccess.Read);
            //BinaryReader binaryReader = new BinaryReader(fs);
            //int read = 0;

            //while (read != 0)
            //{
            //    read = binaryReader.Read(buffer, 0, AuthSession.PacketSize);
            //    AuthSession.Send(buffer);
            //}

            ////Mark the end of the authentication proccess
            //AuthSession.Send(Encoding.UTF8.GetBytes(STOPAUTHENTICATION));
        }

        public AuthenticationObject ReceiveAuthenticationObject(MemoryStream stream, BinaryReader reader)
        {
            int packetSize = AuthSession.PacketSize;
            byte[] buffer = new byte[packetSize];
            byte[] dataReceived = null;
            int read = 0;
            int v = 0;
            string dataString = null;

            AuthenticationObject obj = null;

            while (true)
            {
                read = reader.Read(buffer, 0, packetSize);
                stream.Write(buffer, 0, read);
                dataReceived = stream.ToArray();
                stream.SetLength((stream.Length - stream.Position));

                dataString = Encoding.UTF8.GetString(dataReceived);

                Debug.WriteLine(dataString);
                if (v == 0)
                {
                    obj = AuthenticationObject.GetAthenticationObject(dataString);
                    v++;
                }
                    break;
            }

            return obj;
        }
    }
}
