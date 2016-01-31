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
        
        private static Dictionary<string, RSAParameters> vault = new Dictionary<string, RSAParameters>();

        /// <summary>
        /// Used to acquire a public key and privately store the private key, intended to be used
        /// to establish initial trust with a new account.
        /// </summary>
        /// <returns>The public version of the key</returns>
        public static RSAParameters GetPubKey()
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSAParameters val = RSA.ExportParameters(false);
                vault.Add(Encoding.ASCII.GetString(val.Modulus), RSA.ExportParameters(true));
                return val;
            }
        }

        /// <summary>
        /// Used to acquire the private key to match the public key which is
        /// passed into the method.
        /// </summary>
        /// <param name="pubKey">The public key for which we want the private key</param>
        /// <returns>The private key</returns>
        private static RSAParameters GetSecureKey(RSAParameters pubKey)
        {
            string dictKey = Encoding.ASCII.GetString(pubKey.Modulus);
            if (vault.ContainsKey(dictKey))
            {
                RSAParameters val = vault[dictKey];
                vault.Remove(dictKey);
                return val;
            }
            throw new Exception("Private key not found in dictionary.");
        }

        /// <summary>
        /// Decrypts a string with the secure key that corresponds to the
        /// provided public key
        /// </summary>
        /// <param name="message">The encrypted string</param>
        /// <param name="RSAKeyInfo">The public key, which was generated with this instance of the program</param>
        /// <returns>The decrypted string, empty string on error</returns>
        public static string Decrypt(byte[] message, RSAParameters PublicKey)
        {
            try
            {
                return Encoding.ASCII.GetString(RSADecrypt(message, GetSecureKey(PublicKey), false));
            }
            catch
            {
                return String.Empty;
            }
        }

        static private byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            byte[] decryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(RSAKeyInfo);
                decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
            }
            return decryptedData;
        }
    }
}
