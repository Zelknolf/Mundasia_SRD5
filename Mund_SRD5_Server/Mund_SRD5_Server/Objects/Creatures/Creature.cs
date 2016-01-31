using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.Serialization;

namespace Mundasia.Objects
{
    public partial class Creature
    {
        private static string delimiter = "|";
        private static char[] delim = new char[] { '|' };
        private static string dictDelimiter = "[";
        private static char[] dictDelim = new char[] { '[' };
        private static string keyDelimiter = "]";
        private static char[] keyDelim = new char[] { ']' };

        public DisplayCharacter CachedDisplay = null;

        public Creature() { }

        public Creature(string message)
        {
            string[] split = message.Split(delim);
            if (split.Length < 36) return;
            AccountName = split[0];
            CharacterName = split[1];
            if (!uint.TryParse(split[2], out CharacterRace)) return;
            if (!int.TryParse(split[3], out Sex)) return;
            if (!uint.TryParse(split[4], out Background)) return;

            Classes = new Dictionary<uint, int>();
            foreach(string chCls in split[5].Split(dictDelim))
            {
                if (String.IsNullOrWhiteSpace(chCls)) continue;
                string[] reSplit = chCls.Split(keyDelim);
                if (reSplit.Length != 2) return;
                uint cls;
                int clsLvl;
                if (!uint.TryParse(reSplit[0], out cls)) return;
                if (!int.TryParse(reSplit[1], out clsLvl)) return;
                Classes.Add(cls, clsLvl);
            }

            if (!uint.TryParse(split[6], out ExperiencePoints)) return;
            if (!uint.TryParse(split[7], out Level)) return;
            if (!uint.TryParse(split[8], out Strength)) return;
            if (!uint.TryParse(split[9], out Dexterity)) return;
            if (!uint.TryParse(split[10], out Constitution)) return;
            if (!uint.TryParse(split[11], out Intelligence)) return;
            if (!uint.TryParse(split[12], out Wisdom)) return;
            if (!uint.TryParse(split[13], out Charisma)) return;
            if (!uint.TryParse(split[14], out ProficiencyBonus)) return;

            Skills = new List<uint>();
            foreach(string sk in split[15].Split(dictDelim))
            {
                if (String.IsNullOrWhiteSpace(sk)) continue;
                uint skToAdd;
                if (!uint.TryParse(sk, out skToAdd)) return;
                Skills.Add(skToAdd);
            }
            
            if (!uint.TryParse(split[16], out InspirationPoints)) return;
            if (!uint.TryParse(split[17], out HairStyle)) return;
            if (!uint.TryParse(split[18], out HairColor)) return;
            if (!uint.TryParse(split[19], out SkinColor)) return;
            Map = split[19];
            if (!int.TryParse(split[20], out LocationX)) return;
            if (!int.TryParse(split[21], out LocationY)) return;
            if (!int.TryParse(split[22], out LocationZ)) return;
            if (!Enum.TryParse<Direction>(split[23], out LocationFacing)) return;

            Equipment = new Dictionary<int, InventoryItem>();
            foreach (string equip in split[24].Split(dictDelim))
            {
                if (String.IsNullOrWhiteSpace(equip)) continue;
                string[] reSplit = equip.Split(keyDelim);
                if (reSplit.Length != 2) return;
                int slot;
                if (!int.TryParse(reSplit[0], out slot)) return;
                Equipment.Add(slot, new InventoryItem(reSplit[1]));
            }

            Inventory = new List<InventoryItem>();
            foreach (string it in split[25].Split(dictDelim))
            {
                if (String.IsNullOrWhiteSpace(it)) continue;
                Inventory.Add(new InventoryItem(it));
            }

            if (!bool.TryParse(split[26], out IsGM)) return;
            IsCreatureValid = true;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(AccountName);
            str.Append(delimiter);
            str.Append(CharacterName);
            str.Append(delimiter);
            str.Append(CharacterRace);
            str.Append(delimiter);
            str.Append(Sex);
            str.Append(delimiter);
            str.Append(Background);
            str.Append(delimiter);
            foreach(KeyValuePair<uint, int> chClass in Classes)
            {
                str.Append(chClass.Key);
                str.Append(keyDelimiter);
                str.Append(chClass.Value);
                str.Append(dictDelimiter);
            }
            str.Append(delimiter);
            str.Append(ExperiencePoints);
            str.Append(delimiter);
            str.Append(Level);
            str.Append(delimiter);
            str.Append(Strength);
            str.Append(delimiter);
            str.Append(Dexterity);
            str.Append(delimiter);
            str.Append(Constitution);
            str.Append(delimiter);
            str.Append(Intelligence);
            str.Append(delimiter);
            str.Append(Wisdom);
            str.Append(delimiter);
            str.Append(Charisma);
            str.Append(delimiter);
            str.Append(ProficiencyBonus);
            str.Append(delimiter);
            foreach(uint skill in Skills)
            {
                str.Append(skill);
                str.Append(dictDelimiter);
            }
            str.Append(delimiter);
            str.Append(InspirationPoints);
            str.Append(delimiter);
            str.Append(HairStyle);
            str.Append(delimiter);
            str.Append(HairColor);
            str.Append(delimiter);
            str.Append(SkinColor);
            str.Append(delimiter);
            str.Append(Map);
            str.Append(delimiter);
            str.Append(LocationX);
            str.Append(delimiter);
            str.Append(LocationY);
            str.Append(delimiter);
            str.Append(LocationZ);
            str.Append(delimiter);
            str.Append(LocationFacing);
            str.Append(delimiter);
            foreach(KeyValuePair<int, InventoryItem> equipped in Equipment)
            {
                str.Append(equipped.Key);
                str.Append(keyDelim);
                str.Append(equipped.Value.ToString());
                str.Append(dictDelim);
            }
            str.Append(delimiter);
            foreach(InventoryItem item in Inventory)
            {
                str.Append(item.ToString());
                str.Append(dictDelim);
            }
            str.Append(delimiter);
            str.Append(IsGM);
            return str.ToString();
        }

        public bool IsCreatureValid = false;
        
        [XmlElement]
        public string AccountName;

        [XmlElement]
        public string CharacterName;

        [XmlElement]
        public uint CharacterRace;

        [XmlElement]
        public int Sex;

        [XmlElement]
        public uint Background;

        [XmlArray]
        public Dictionary<uint, int> Classes;

        [XmlElement]
        public uint ExperiencePoints;

        [XmlElement]
        public uint Level;

        [XmlElement]
        public uint Strength;

        [XmlElement]
        public uint Dexterity;

        [XmlElement]
        public uint Constitution;

        [XmlElement]
        public uint Intelligence;

        [XmlElement]
        public uint Wisdom;

        [XmlElement]
        public uint Charisma;

        [XmlElement]
        public uint ProficiencyBonus;

        [XmlArray]
        public List<uint> Skills;

        [XmlElement]
        public uint InspirationPoints;

        [XmlElement]
        public uint HairStyle;

        [XmlElement]
        public uint HairColor;

        [XmlElement]
        public uint SkinColor;

        [XmlElement]
        public string Map;

        [XmlElement]
        public int LocationX;

        [XmlElement]
        public int LocationY;

        [XmlElement]
        public int LocationZ;

        [XmlElement]
        public Direction LocationFacing;

        [XmlArray]
        public Dictionary<int, InventoryItem> Equipment;

        [XmlArray]
        public List<InventoryItem> Inventory;

        [XmlElement]
        public bool IsGM;

        private static Dictionary<uint, uint> _pointValue = new Dictionary<uint, uint>()
        {
            { 6, 0 },
            { 7, 1 },
            { 8, 2 },
            { 9, 3 },
            { 10, 4 },
            { 11, 5 },
            { 12, 6 },
            { 13, 8 },
            { 14, 10 },
            { 15, 12 },
            { 16, 14 },
            { 17, 17 },
            { 18, 20 },
        };

        public bool ValidateCharacter()
        {
            foreach(char ch in Path.GetInvalidFileNameChars())
            {
                if(CharacterName.Contains(ch))
                {
                    return false;
                }
            }
            if(Sex != 0 && Sex != 1)
            {
                return false;
            }
            Race r = Race.GetRace(CharacterRace);
            if(r == null)
            {
                return false;
            }
            if(!_pointValue.ContainsKey(Strength))
            {
                return false;
            }
            if(!_pointValue.ContainsKey(Dexterity))
            {
                return false;
            }
            if (!_pointValue.ContainsKey(Constitution))
            { 
                return false; 
            }
            if(!_pointValue.ContainsKey(Intelligence))
            {
                return false;
            }
            if(!_pointValue.ContainsKey(Wisdom))
            {
                return false;
            }
            if(!_pointValue.ContainsKey(Charisma))
            {
                return false;
            }

            if(_pointValue[Strength] + _pointValue[Dexterity] + _pointValue[Constitution] + _pointValue[Intelligence] + _pointValue[Wisdom] + _pointValue[Charisma] > 40)
            {
                return false;
            }

            LocationX = 20000000;
            LocationY = 20000000;
            LocationZ = 0;
            LocationFacing = Direction.North;
            Map = "Material";
            return true;
        }
    }
}
