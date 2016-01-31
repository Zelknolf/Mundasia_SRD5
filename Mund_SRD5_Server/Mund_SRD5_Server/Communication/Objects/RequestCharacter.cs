using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundasia.Communication
{
    public class RequestCharacter
    {
        private static string delimiter = "|";
        private static char[] delim = new char[] { '|' };
        
        public RequestCharacter() { }

        public RequestCharacter(string message)
        {
            string[] split = message.Split(delim);
            UserId = split[0];
            if(!int.TryParse(split[1], out SessionId))
            {
                SessionId = int.MaxValue;
            }
            RequestedCharacter = split[2];
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(UserId);
            str.Append(delimiter);
            str.Append(SessionId.ToString());
            str.Append(delimiter);
            str.Append(RequestedCharacter);
            return str.ToString();
        }

        public string UserId;
        public int SessionId;
        public string RequestedCharacter;
    }
}
