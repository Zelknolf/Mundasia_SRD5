using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundasia.Objects
{
    public class InventoryItem
    {
        private static string delimiter = "]";
        private static char[] delim = new char[] { ']' };

        public InventoryItem() { }

        public InventoryItem(string FileLine) 
        {
            string[] pieces = FileLine.Split(delim);
            if (pieces.Length < 4) return;

            int.TryParse(pieces[0], out EquipKey);
            ItemType.TryParse(pieces[1], out ItType);
            int.TryParse(pieces[2], out Appearance);
            int.TryParse(pieces[3], out PrimaryColor);
            int.TryParse(pieces[4], out SecondaryColor);
            Name = pieces[5];
            Identifier = pieces[6];
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(EquipKey);
            str.Append(delimiter);
            str.Append(ItType);
            str.Append(delimiter);
            str.Append(Appearance);
            str.Append(delimiter);
            str.Append(PrimaryColor);
            str.Append(delimiter);
            str.Append(SecondaryColor);
            str.Append(delimiter);
            str.Append(Name);
            str.Append(delimiter);
            str.Append(Identifier);
            return str.ToString();
        }

        public int EquipKey;
        public ItemType ItType;

        public string Name;
        public string Identifier;
        public string Icon;

        public int Appearance;
        public int PrimaryColor;
        public int SecondaryColor;
    }

    public enum ItemType
    {
        Clothing = 0,
        Necklace = 1,
        Belt = 2,
        Ring = 3,
        OneHand = 4,
        TwoHand = 5,
        OffHand = 6,
        Misc = 99,
    }

    public enum InventorySlot
    {
        Chest = 0,
        Neck = 1,
        Belt = 2,
        LeftRing = 3,
        RightRing = 4,
        LeftHand = 5,
        RightHand = 6,
    }
}
