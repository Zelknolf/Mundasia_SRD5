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
            
        }

        public override string ToString()
        {
            return String.Empty;
        }

        public bool IsCreatureValid = false;

        public int ArmorClass
        {
            get
            {
                // TODO: Math.Min(max dex, dex)
                int val = 10 + getMod(Dexterity);
                
                // TODO: Powers need a means to hook here, for unarmored defense.

                // TODO: AC from gear

                return val;
            }
        }

        [XmlElement]
        public string AccountName;

        [XmlElement]
        public string CharacterName;

        [XmlElement]
        public Alignment CharacterAlignment;

        [XmlElement]
        public Race CharacterRace;

        [XmlElement]
        public int Gender;

        [XmlElement]
        public Background Background;

        [XmlArray]
        public Dictionary<CharacterClass, uint> Classes;

        [XmlArray]
        public Dictionary<CharacterClass, CharacterClass> SubClasses;

        [XmlArray]
        public List<uint> ProficientSaves;

        [XmlArray]
        public List<Spell> Cantrips;

        [XmlArray]
        public List<Spell> SpellsKnown;

        [XmlElement]
        public uint ExperiencePoints;

        [XmlElement]
        public uint Level;

        [XmlElement]
        public int Strength;

        [XmlElement]
        public int Dexterity;

        [XmlElement]
        public int Constitution;

        [XmlElement]
        public int Intelligence;

        [XmlElement]
        public int Wisdom;

        [XmlElement]
        public int Charisma;

        [XmlElement]
        public uint ProficiencyBonus;

        [XmlArray]
        public List<Skill> Skills;

        [XmlArray]
        public List<Power> Powers;

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

        public bool ValidateCharacter()
        {
            foreach(char ch in Path.GetInvalidFileNameChars())
            {
                if(CharacterName.Contains(ch))
                {
                    return false;
                }
            }
            if(Gender != 0 && Gender != 1)
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

        private int getMod(int ability)
        {
            if(ability > 10)
            {
                return (ability - 10) / 2;
            }
            else
            {
                return (ability - 11) / 2;
            }
        }
    }
}
