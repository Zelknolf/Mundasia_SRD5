using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundasia.Communication
{
    public class Login
    {
        // We use a printable character as a delimiter to make sure that the end format is something simple and
        // describable.
        private static string delimiter = "|";
        private static char[] delim = new char[] { '|' };

        public string password;
        public string userName;

        public Login() { }

        public Login(string messageString)
        {
            string[] piecedString = messageString.Split(delim);
            if (piecedString.Length != 2)
            {
                throw new Exception("Could not translate string into a login object.");
            }
            userName = piecedString[0];
            password = piecedString[1];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(userName);
            sb.Append(delimiter);
            sb.Append(password);
            return sb.ToString();
        }
    }
}
