using System;
using System.Security.Cryptography;
using System.Text;

namespace ReChat.Helpers
{
    class Security
    {
        public static string getMd5Hash(string input)
        {
            if (input == null)
                return null;

            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            string hash = "";

            foreach (byte b in data)
                hash += string.Format("{0:x2}", b);

            return hash;
        }

        public static string GetToken()
        {
            return Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
        }


    }
}
