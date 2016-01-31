using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundasia.Communication
{
    public class EquipRequest
    {
        private static string delimiter = "|";
        private static char[] delim = new char[] { '|' };

        public string RequestingCharacter;
        public string RequestingAccount;
        public string ChangedCharacter;
        public string ChangedAccount;
        public string Identifier;
        public int InventorySlot;
        public int SessionId;

        public EquipRequest() { }

        public EquipRequest(string fileLine)
        {
            string[] pieces = fileLine.Split(delim);
            if (pieces.Length < 6) return;
            RequestingCharacter = pieces[0];
            RequestingAccount = pieces[1];
            ChangedCharacter = pieces[2];
            ChangedAccount = pieces[3];
            Identifier = pieces[4];
            Int32.TryParse(pieces[5], out InventorySlot);
        }

        public EquipRequest(string requestingCharacter, string requestingAccount, string changedCharacter, string changedAccount, string tag, int inventorySlot, int sessionId)
        {
            RequestingCharacter = requestingCharacter;
            RequestingAccount = requestingAccount;
            ChangedCharacter = changedCharacter;
            ChangedAccount = changedAccount;
            Identifier = tag;
            InventorySlot = inventorySlot;
            SessionId = sessionId;
        }

        public override string ToString()
        {
            StringBuilder bld = new StringBuilder();
            bld.Append(RequestingCharacter);
            bld.Append(delimiter);
            bld.Append(RequestingAccount);
            bld.Append(delimiter);
            bld.Append(ChangedCharacter);
            bld.Append(delimiter);
            bld.Append(ChangedAccount);
            bld.Append(delimiter);
            bld.Append(Identifier);
            bld.Append(delimiter);
            bld.Append(InventorySlot);
            bld.Append(delimiter);
            return bld.ToString();
        }
    }
}
