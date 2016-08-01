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
        private static string listDelimiter = "[";
        private static char[] listDelim = new char[] { '[' };
        private static string keyDelimiter = "]";
        private static char[] keyDelim = new char[] { ']' };

        public DisplayCharacter CachedDisplay = null;

        public Creature() { }

        public Creature(string message)
        {
            string[] buildSplit = message.Split(delim);
            AccountName = buildSplit[0];
            CharacterName = buildSplit[1];
            CharacterAlignment = Alignment.GetAlignment(uint.Parse(buildSplit[2]));
            CharacterRace = Race.GetRace(uint.Parse(buildSplit[3]));
            Gender = int.Parse(buildSplit[4]);
            Background = Background.GetBackground(uint.Parse(buildSplit[5]));

            string[] classSplit = buildSplit[6].Split(listDelim);
            Classes = new Dictionary<CharacterClass, uint>();
            foreach(string cl in classSplit)
            {
                if(String.IsNullOrWhiteSpace(cl))
                {
                    continue;
                }
                string[] clSp = cl.Split(keyDelim);
                CharacterClass toAdd = CharacterClass.GetClass(uint.Parse(clSp[0]));
                uint level = uint.Parse(clSp[1]);
                Classes.Add(toAdd, level);
            }

            string[] subClassSplit = buildSplit[7].Split(listDelim);
            SubClasses = new Dictionary<CharacterClass, CharacterClass>();
            foreach (string sub in subClassSplit)
            {
                if (String.IsNullOrWhiteSpace(sub))
                {
                    continue;
                }
                string[] subSp = sub.Split(keyDelim);
                CharacterClass baseClass = CharacterClass.GetClass(uint.Parse(subSp[0]));
                if (!String.IsNullOrWhiteSpace(subSp[1]))
                {
                    CharacterClass subClass = CharacterClass.GetClass(uint.Parse(subSp[1]));
                    SubClasses.Add(baseClass, subClass);
                }
                else
                {
                    SubClasses.Add(baseClass, null);
                }
            }

            string[] saveSplit = buildSplit[8].Split(listDelim);
            ProficientSaves = new List<uint>();
            foreach (string save in saveSplit)
            {
                if(String.IsNullOrWhiteSpace(save))
                {
                    continue;
                }
                ProficientSaves.Add(uint.Parse(save));
            }

            string[] cantripSplit = buildSplit[9].Split(listDelim);
            Cantrips = new List<Spell>();
            foreach(string cantrip in cantripSplit)
            {
                if(String.IsNullOrWhiteSpace(cantrip))
                {
                    continue;
                }
                Cantrips.Add(Spell.GetSpell(uint.Parse(cantrip)));
            }

            string[] spellSplit = buildSplit[10].Split(listDelim);
            SpellsKnown = new List<Spell>();
            foreach(string spell in spellSplit)
            {
                if(String.IsNullOrWhiteSpace(spell))
                {
                    continue;
                }
                SpellsKnown.Add(Spell.GetSpell(uint.Parse(spell)));
            }

            ExperiencePoints = uint.Parse(buildSplit[11]);
            Level = uint.Parse(buildSplit[12]);
            Strength = int.Parse(buildSplit[13]);
            Dexterity = int.Parse(buildSplit[14]);
            Constitution = int.Parse(buildSplit[15]);
            Intelligence = int.Parse(buildSplit[16]);
            Wisdom = int.Parse(buildSplit[17]);
            Charisma = int.Parse(buildSplit[18]);
            ProficiencyBonus = uint.Parse(buildSplit[19]);

            string[] skillSplit = buildSplit[20].Split(listDelim);
            Skills = new List<Skill>();
            foreach(string skill in skillSplit)
            {
                if(String.IsNullOrWhiteSpace(skill))
                {
                    continue;
                }
                Skills.Add(Skill.GetSkill(uint.Parse(skill)));
            }

            string[] powerSplit = buildSplit[21].Split(listDelim);
            Powers = new List<Power>();
            foreach(string power in powerSplit)
            {
                if(String.IsNullOrWhiteSpace(power))
                {
                    continue;
                }
                Powers.Add(Power.GetPower(uint.Parse(power)));
            }

            InspirationPoints = uint.Parse(buildSplit[22]);
            HairStyle = uint.Parse(buildSplit[23]);
            HairColor = uint.Parse(buildSplit[24]);
            SkinColor = uint.Parse(buildSplit[25]);
            Map = buildSplit[26];
            LocationX = int.Parse(buildSplit[27]);
            LocationY = int.Parse(buildSplit[28]);
            LocationZ = int.Parse(buildSplit[29]);
            LocationFacing = (Direction)Enum.Parse(typeof(Direction), buildSplit[30]);

            string[] equipSplit = buildSplit[31].Split(listDelim);
            Equipment = new Dictionary<int, InventoryItem>();
            foreach(string equip in equipSplit)
            {
                if(String.IsNullOrWhiteSpace(equip))
                {
                    continue;
                }
                string[] itemSplit = equip.Split(keyDelim);
                InventoryItem it = new InventoryItem(itemSplit[1]);
                if (it.Valid)
                {
                    Equipment.Add(int.Parse(itemSplit[0]), it);
                }
            }

            string[] invSplit = buildSplit[32].Split(listDelim);
            Inventory = new List<InventoryItem>();
            foreach(string item in invSplit)
            {
                if(String.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                InventoryItem it = new InventoryItem(item);
                if (it.Valid)
                {
                    Inventory.Add(it);
                }
            }
            IsGM = bool.Parse(buildSplit[33]);
            IsCreatureValid = true;
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.Append(AccountName);
            ret.Append(delimiter);
            ret.Append(CharacterName);
            ret.Append(delimiter);
            ret.Append(CharacterAlignment.Id);
            ret.Append(delimiter);
            ret.Append(CharacterRace.Id);
            ret.Append(delimiter);
            ret.Append(Gender);
            ret.Append(delimiter);
            ret.Append(Background.Id);
            ret.Append(delimiter);
            foreach(KeyValuePair<CharacterClass, uint> ch in Classes)
            {
                ret.Append(ch.Key.Id);
                ret.Append(keyDelimiter);
                ret.Append(ch.Value);
                ret.Append(listDelimiter);
            }
            ret.Append(delimiter);
            foreach(KeyValuePair<CharacterClass,CharacterClass> sub in SubClasses)
            {
                ret.Append(sub.Key.Id);
                ret.Append(keyDelimiter);
                if (sub.Value != null)
                {
                    ret.Append(sub.Value.Id);
                }
                else
                {
                    ret.Append(String.Empty);
                }
                ret.Append(listDelimiter);
            }
            ret.Append(delimiter);
            foreach(uint save in ProficientSaves)
            {
                ret.Append(save);
                ret.Append(listDelimiter);
            }
            ret.Append(delimiter);
            foreach(Spell sp in Cantrips)
            {
                ret.Append(sp.Id);
                ret.Append(listDelimiter);
            }
            ret.Append(delimiter);
            foreach(Spell sp in SpellsKnown)
            {
                ret.Append(sp.Id);
                ret.Append(listDelimiter);
            }
            ret.Append(delimiter);
            ret.Append(ExperiencePoints);
            ret.Append(delimiter);
            ret.Append(Level);
            ret.Append(delimiter);
            ret.Append(Strength);
            ret.Append(delimiter);
            ret.Append(Dexterity);
            ret.Append(delimiter);
            ret.Append(Constitution);
            ret.Append(delimiter);
            ret.Append(Intelligence);
            ret.Append(delimiter);
            ret.Append(Wisdom);
            ret.Append(delimiter);
            ret.Append(Charisma);
            ret.Append(delimiter);
            ret.Append(ProficiencyBonus);
            ret.Append(delimiter);
            foreach(Skill sk in Skills)
            {
                ret.Append(sk.Id);
                ret.Append(listDelimiter);
            }
            ret.Append(delimiter);
            foreach(Power pw in Powers)
            {
                ret.Append(pw.Id);
                ret.Append(listDelimiter);
            }
            ret.Append(delimiter);
            ret.Append(InspirationPoints);
            ret.Append(delimiter);
            ret.Append(HairStyle);
            ret.Append(delimiter);
            ret.Append(HairColor);
            ret.Append(delimiter);
            ret.Append(SkinColor);
            ret.Append(delimiter);
            ret.Append(Map);
            ret.Append(delimiter);
            ret.Append(LocationX);
            ret.Append(delimiter);
            ret.Append(LocationY);
            ret.Append(delimiter);
            ret.Append(LocationZ);
            ret.Append(delimiter);
            ret.Append(LocationFacing);
            ret.Append(delimiter);
            foreach(KeyValuePair<int, InventoryItem> it in Equipment)
            {
                if (it.Value != null)
                {
                    ret.Append(it.Key);
                    ret.Append(keyDelimiter);
                    ret.Append(it.Value.ToString());
                    ret.Append(listDelimiter);
                }
            }
            ret.Append(delimiter);
            foreach(InventoryItem it in Inventory)
            {
                ret.Append(it.ToString());
                ret.Append(listDelimiter);
            }
            ret.Append(delimiter);
            ret.Append(IsGM);
            return ret.ToString();
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
