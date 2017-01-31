using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Seed.Shared;

namespace Seed.Cryptography
{
    public class MD5Cryptographer : ICryptographer
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
        
        byte[] ICryptographer.Encrypt(byte[] data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(initVector);
            byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
            byte[] buffer = data;
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
            return inArray;
        }

        byte[] ICryptographer.Decrypt(byte[] data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(initVector);
            byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
            byte[] buffer = data;
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
            return buffer5;
        }
    }
}
