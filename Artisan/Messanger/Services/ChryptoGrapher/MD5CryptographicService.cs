using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Seed.Shared;

namespace Messenger.Services.ChryptoGrapher
{
    public class MD5CryptographicService 
    {
        static string passPhrase = "UUIuasbd qjke1803291-494823@!%$&@*#(__!+!+~(&#YUGKWKWwidn,c z" +
            ",fdsm??//\\\"\'vdm`~~':'l;';[]{";
        static string saltValue = "sALtValue";
        static string hashAlgorithm = "MD5";
        static int passwordIterations = 7;
        static string initVector = "~1B2c3D4e5F6g7H8";
        static int keySize = 256;


        static public string PassPhrase { get { return passPhrase; } set { passPhrase = value; } }
        static public string SaltValue { get { return saltValue; } set { saltValue = value; } }

        public static string Encrypt(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(initVector);
            byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            byte[] rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
            RijndaelManaged managed = new RijndaelManaged();
            managed.Mode = CipherMode.CBC;
            ICryptoTransform transform = managed.CreateEncryptor(rgbKey, bytes);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            byte[] inArray = stream.ToArray();
            stream.Close();
            stream2.Close();
            return Convert.ToBase64String(inArray);
        }

        public static string Decrypt(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(initVector);
            byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
            byte[] buffer = Convert.FromBase64String(data);
            byte[] rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
            RijndaelManaged managed = new RijndaelManaged();
            managed.Mode = CipherMode.CBC;
            ICryptoTransform transform = managed.CreateDecryptor(rgbKey, bytes);
            MemoryStream stream = new MemoryStream(buffer);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            byte[] buffer5 = new byte[buffer.Length];
            int count = stream2.Read(buffer5, 0, buffer5.Length);
            stream.Close();
            stream2.Close();
            return Encoding.UTF8.GetString(buffer5, 0, count);
        }
    }
}
