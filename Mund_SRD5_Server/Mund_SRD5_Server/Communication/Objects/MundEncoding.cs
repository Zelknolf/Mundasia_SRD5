using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundasia.Communication
{
    public class MundEncoding
    {
        /// <summary>
        /// Convert a string to bytes, assuming that the string presents the bytes in hexadecimal
        /// format.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] StringToBytes(string input)
        {
            byte[] bytes = new byte[input.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(input.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        public static byte[] GetPasswordSalt(string userName, string password)
        {
            string salt = userName + DateTime.UtcNow.Date.ToShortDateString();
            return MundEncoding.StringToBytes(salt);
        }
    }
}
