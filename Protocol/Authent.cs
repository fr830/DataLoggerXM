using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Collections;
using System.Linq;
namespace Protocol
{
   public static class Authent

    {
        //private static string _privateKey ;
        //private static string _publicKey ;
        private static ASCIIEncoding _encoder = new ASCIIEncoding();

        public static string Au(string _privateKey, string _publicKey, String enc)
        {
            var rsa = new RSACryptoServiceProvider();
            _privateKey = rsa.ToXmlString(true);
            _publicKey = rsa.ToXmlString(false);

            //var text = "Test1";
            //Console.WriteLine("RSA // Text to encrypt: " + text);
            //var enc = Encrypt(text);
            //Console.WriteLine("RSA // Encrypted Text: " + enc);
            var dec = Decrypt(enc, _privateKey);
            return dec;
            //Console.WriteLine("RSA // Decrypted Text: " + dec);
           // Console.ReadLine();
        }

        public static string Decrypt(string data, string _privateKey)
        {
            var rsa = new RSACryptoServiceProvider();
            var dataArray = data.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            rsa.FromXmlString(_privateKey);
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }
        public static string Encrypt(string data, string _publicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_publicKey);
            var dataToEncrypt = _encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            var length = encryptedByteArray.Count();
            var item = 0;
            var sb = new StringBuilder();
            foreach (var x in encryptedByteArray)
            {
                item++;
                sb.Append(x);
                if (item < length)
                    sb.Append(",");
            }
            return sb.ToString();
        }
    }
}