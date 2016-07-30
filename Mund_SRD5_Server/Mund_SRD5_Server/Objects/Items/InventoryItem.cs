using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Mundasia.Objects
{
    public class InventoryItem
    {
        private static int identifier = 0;
        private static string delimiter = "]";
        private static char[] delim = new char[] { ']' };

        public InventoryItem() { }

        public InventoryItem(string FileLine)
        {
            Valid = false;
            string[] pieces = FileLine.Split(delim);
            if (pieces.Length < 4) return;

            if (!Enum.TryParse<ItemType>(pieces[0], out ItType))
            {
                return;
            }

            Name = pieces[1];

            if (String.IsNullOrEmpty(pieces[2]))
            {
                Identifier = (identifier++).ToString("X20");
            }
            else
            {
                Identifier = pieces[2];
            }

            Icon = pieces[3];

            if (!int.TryParse(pieces[4], out Appearance))
            {
                return;
            }

            if (!int.TryParse(pieces[5], out PrimaryColor))
            {
                return;
            }

            if (!int.TryParse(pieces[6], out SecondaryColor))
            {
                return;
            }

            if (!int.TryParse(pieces[7], out CostInCopper))
            {
                return;
            }

            if (!Enum.TryParse<ArmorWeight>(pieces[8], out ArmorWeight))
            {
                return;
            }

            if (!int.TryParse(pieces[9], out BaseArmorClass))
            {
                return;
            }

            if (!int.TryParse(pieces[10], out MaxDexBonus))
            {
                return;
            }

            if(!int.TryParse(pieces[11], out ModArmorClass))
            {
                return;
            }

            if(!bool.TryParse(pieces[12], out Finesse))
            {
                return;
            }

            if(!bool.TryParse(pieces[13], out Loading))
            {
                return;
            }

            if(!bool.TryParse(pieces[14], out Noisy))
            {
                return;
            }

            if(!int.TryParse(pieces[15], out CloseRangeInTiles))
            {
                return;
            }

            if (!int.TryParse(pieces[16], out LongRangeInTiles))
            {
                return;
            }
            
            if(!int.TryParse(pieces[17], out ReachInTiles))
            {
                return;
            }
            
            if(!int.TryParse(pieces[18], out WeightInOunces))
            {
                return;
            }

            if(!int.TryParse(pieces[19], out DonTimeSeconds))
            {
                return;
            }

            if(!int.TryParse(pieces[20], out DoffTimeSeconds))
            {
                return;
            }
            
            if(!int.TryParse(pieces[21], out DamageDiceNumber))
            {
                return;
            }

            if(!int.TryParse(pieces[22], out DamageDiceNumber))
            {
                return;
            }

            if(!int.TryParse(pieces[23], out VersatileDamageDiceNumber))
            {
                return;
            }

            if(!int.TryParse(pieces[24], out VersatileDamageDiceType))
            {
                return;
            }

            Valid = true;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(ItType).Append(delimiter);
            str.Append(Name).Append(delimiter);
            str.Append(Identifier).Append(delimiter);
            str.Append(Icon).Append(delimiter);
            str.Append(Appearance).Append(delimiter);
            str.Append(PrimaryColor).Append(delimiter);
            str.Append(SecondaryColor).Append(delimiter);
            str.Append(CostInCopper).Append(delimiter);
            str.Append(ArmorWeight).Append(delimiter);
            str.Append(BaseArmorClass).Append(delimiter);
            str.Append(MaxDexBonus).Append(delimiter);
            str.Append(ModArmorClass).Append(delimiter);
            str.Append(Finesse).Append(delimiter);
            str.Append(Loading).Append(delimiter);
            str.Append(Noisy).Append(delimiter);
            str.Append(CloseRangeInTiles).Append(delimiter);
            str.Append(LongRangeInTiles).Append(delimiter);
            str.Append(ReachInTiles).Append(delimiter);
            str.Append(WeightInOunces).Append(delimiter);
            str.Append(DonTimeSeconds).Append(delimiter);
            str.Append(DoffTimeSeconds).Append(delimiter);
            str.Append(DamageDiceNumber).Append(delimiter);
            str.Append(DamageDiceType).Append(delimiter);
            str.Append(VersatileDamageDiceNumber).Append(delimiter);
            str.Append(VersatileDamageDiceType).Append(delimiter);
            return str.ToString();
        }

        public ItemType ItType;

        public string Name;
        public string Identifier;
        public string Icon;

        public int Appearance;
        public int PrimaryColor;
        public int SecondaryColor;

        public bool Valid;

        public int CostInCopper;

        public ArmorWeight ArmorWeight;
        public int BaseArmorClass;
        public int MaxDexBonus;
        public int ModArmorClass;

        public bool Finesse;
        public bool Loading;

        public bool Noisy;

        public int CloseRangeInTiles;
        public int LongRangeInTiles;
        public int ReachInTiles;

        public int WeightInOunces;

        public int DonTimeSeconds;
        public int DoffTimeSeconds;

        public int DamageDiceNumber;
        public int DamageDiceType;

        public int VersatileDamageDiceNumber;
        public int VersatileDamageDiceType;


        private static Dictionary<string, InventoryItem> _library = new Dictionary<string, InventoryItem>();

        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\BaseItems.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    InventoryItem toAdd = new InventoryItem(read.ReadLine());
                    if(toAdd.Valid)
                    {
                        _library.Add(toAdd.Identifier, toAdd);
                    }
                }
            }
        }
    }

    public enum ArmorWeight
    {
        Cloth = 0,
        Light = 1,
        Medium = 2,
        Heavy = 3,
    }

    public enum ItemType
    {
        Clothing = 0,
        Necklace = 1,
        Belt = 2,
        Ring = 3,
        Weapon = 4,
        HeavyOrAmmoWeapon = 5,
        LightWeapon = 6,
        Brooch = 7,
        Helm = 8,
        Lens = 9,
        Cloak = 10,
        Stone = 11,
        Bracers = 12,
        Gloves = 13,
        Boots = 14,
        Shield = 15,
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
        Brooch = 7,
        Helm = 8,
        Lens = 9,
        Cloak = 10,
        Stone = 11,
        Bracers = 12,
        Gloves = 13,
        Boots = 14,
    }
}
