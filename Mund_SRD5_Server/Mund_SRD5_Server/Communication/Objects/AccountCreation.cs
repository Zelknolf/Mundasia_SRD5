using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Mundasia.Communication
{
    public class AccountCreation
    {
        // We use a printable character as a delimiter to make sure that the end format is something simple and
        // describable.
        private static string delimiter = "|";
        private static char[] delim = new char[] { '|' };

        public byte[] message;
        public RSAParameters pubKey;

        /// <summary>
        /// Create a new AccountCreation object with arbitrary values.
        /// </summary>
        public AccountCreation() { }

        /// <summary>
        /// Create an AccountCreation object from a string as it is provided by AccountCreation's ToString
        /// method.
        /// </summary>
        /// <param name="messageString">The string as it is produced by ToString()</param>
        public AccountCreation(string messageString)
        {
            string[] piecedString = messageString.Split(delim);
            if (piecedString.Length != 3)
            {
                throw new Exception("Could not translate string into an account creation object.");
            }
            if(piecedString[0].Length > 0)
            {
                message = MundEncoding.StringToBytes(piecedString[0]);
            }
            else
            {
                message = new byte[0];
            }
            pubKey.Modulus = MundEncoding.StringToBytes(piecedString[1]);
            pubKey.Exponent = MundEncoding.StringToBytes(piecedString[2]);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(byte b in message)
            {
                sb.Append(b.ToString("X2"));
            }
            sb.Append(delimiter);
            foreach (byte b in pubKey.Modulus)
            {
                sb.Append(b.ToString("X2"));
            }
            sb.Append(delimiter);
            foreach(byte b in pubKey.Exponent)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
