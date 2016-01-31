using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;
using System.IO;
using System.ComponentModel;

namespace Mundasia.Communication
{
    public partial class Encryption
    {
        /// <summary>
        /// Transforms the password into a hash, to make the saved string less-accessible than it would be otherwise.
        /// Presumably the machine itself would also be secure, and we're not going to do anything dumb like put these
        /// on a SQL table that enjoys code injections.
        /// 
        /// Portions of the code which intend to authenticate based on user input should still use public key/secure 
        /// key combinations to secure the sending, otherwise this is still vulnerable to a repeater attack.
        /// </summary>
        /// <param name="password">the password to be hashed</param>
        /// <returns>the hashed password</returns>
        public static string GetSha256Hash(string password)
        {
            HashAlgorithm alg = SHA256.Create();
            byte[] hashByte = alg.ComputeHash(Encoding.ASCII.GetBytes(password));
            StringBuilder ret = new StringBuilder();
            foreach (byte b in hashByte)
                ret.Append(b.ToString("X2"));

            return ret.ToString();
        }

        /// <summary>
        /// Encrypt a string based on the public key provided.
        /// </summary>
        /// <param name="message">The string to encrypt</param>
        /// <param name="RSAKeyInfo">The public key to use</param>
        /// <returns>The encrypted string, empty array on error</returns>
        public static byte[] Encrypt(string message, RSAParameters PublicKey)
        {
            try
            {
                return RSAEncrypt(Encoding.ASCII.GetBytes(message), PublicKey, false);
            }
            catch
            {
                return new byte[1];
            }
        }

        static private byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            byte[] encryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(RSAKeyInfo);
                encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            return encryptedData;
        }
    }
}
