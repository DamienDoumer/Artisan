using System;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace Seed.Shared
{
    public class Document
    {
        public string Name { get; set; }
        /// <summary>
        /// Determines wether the file is a user's picture or not.
        /// </summary>
        public bool isUserPic { get; set; }
        public string IP { get; set; }
        public long Size { get; set; }
        public string DocPath { get; set; }
        public int NumberOfPackets { get; }
        public int PacketSize { get; }

        public Document(string path, string ip, int packetSize, long size)
        {
            PacketSize = packetSize;
            IP = ip;
            isUserPic = false;
            Name = Path.GetFileName(path);
            Size = size;
            //Calculates the number of packets in which the file willl be sent
            NumberOfPackets = 
                Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Size) / 
                Convert.ToDouble(PacketSize)));

            DocPath = path;
        }

        public static string JsonSerialize(Document doc)
        {
            return JsonConvert.SerializeObject(doc);
        }
        public static Document JsonDeserialize(string doc)
        {
            return JsonConvert.DeserializeObject<Document>(doc);
        }
    }
}
