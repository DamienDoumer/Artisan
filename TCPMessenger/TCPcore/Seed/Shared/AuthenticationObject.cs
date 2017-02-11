using System;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace Seed.Shared
{
    public class AuthenticationObject
    {
        public string Name { get; private set; }
        public string IP { get; private set; }
        private byte[] picture;
        public string ConversionPath { get; set; }
        public string PicturePath { get; set; }
        /// <summary>
        /// this property indicates whether the sender of this authentication object 
        /// is authenticated to the receiver already. if yes then set it to true else set it to false
        /// </summary>
        public bool Authenticated { get; set; }

        public AuthenticationObject(string name, string IP, String picPath)
        {
            Name = name;
            this.IP = IP;
            ConversionPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            PicturePath = picPath;
            Authenticated = false;
        }

        private byte[] ConvertImageToByte(string path)
        {
            byte[] fileContent = null;

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fs);

            long byteLength = new FileInfo(path).Length;
            fileContent = binaryReader.ReadBytes((Int32)byteLength);
            fs.Close();
            fs.Dispose();
            binaryReader.Close();

            return fileContent;
        }

        //private void ConvertByteToImage()
        //{
        //    FileStream fStream = new FileStream(ConversionPath, FileMode.Create, FileAccess.Write);

        //    BinaryWriter binaryWriter = new BinaryWriter(fStream);

        //    long byteLength = Picture.Length;
        //    binaryWriter.Write(Picture);
        //    fStream.Close();
        //    fStream.Dispose();
        //    binaryWriter.Close();
        //}

        /// <summary>
        /// Convert this object to string entirely to be sent in the strean
        /// </summary>
        /// <returns></returns>
        public static string GetJsonString(AuthenticationObject obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Converts a Json string into Authentication object
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static AuthenticationObject GetAthenticationObject(string jsonString)
        {
            return JsonConvert.DeserializeObject<AuthenticationObject>(jsonString);
        }
    }
}
