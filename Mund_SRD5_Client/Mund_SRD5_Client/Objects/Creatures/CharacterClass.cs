﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.IO;

namespace Mundasia.Objects
{
    public class CharacterClass
    {
        const char delim = '|';

        const char listDelim = '[';

        const char dictDelim = ']';

        public CharacterClass(string fileLine)
        {
            string[] split = fileLine.Split(delim);

            uint id = 0;
            if(uint.TryParse(split[0], out id))
            {
                Id = id;
                Name = split[1];

                HitDie = Int32.Parse(split[2]);
                SubClassLevel = Int32.Parse(split[3]);

                string[] subClassSplit = split[4].Split(listDelim);
                foreach(string subClass in subClassSplit)
                {
                    if (String.IsNullOrWhiteSpace(subClass)) continue;
                    _subClassIds.Add(uint.Parse(subClass));
                }

                SkillChoices = Int32.Parse(split[5]);
                string[] skillSplit = split[6].Split(listDelim);
                foreach(string skill in skillSplit)
                {
                    if (String.IsNullOrWhiteSpace(skill)) continue;
                    Skill sk = Skill.GetSkill(uint.Parse(skill));
                    if (sk != null) AvailableSkills.Add(sk);
                }

                ToolChoices = Int32.Parse(split[7]);
                string[] toolSplit = split[8].Split(listDelim);
                foreach(string tool in toolSplit)
                {
                    if (String.IsNullOrWhiteSpace(tool)) continue;
                    Skill sk = Skill.GetSkill(uint.Parse(tool));
                    if (sk != null) AvailableTools.Add(sk);
                }

                string[] powerSplit = split[9].Split(dictDelim);
                foreach(string power in powerSplit)
                {
                    if (String.IsNullOrWhiteSpace(power)) continue;
                    string[] indexedPower = power.Split(listDelim);
                    uint index = uint.Parse(indexedPower[0]);
                    if (!ClassPowers.ContainsKey(index))
                    {
                        ClassPowers.Add(index, new List<List<Power>>());
                    }
                    List<Power> toAdd = new List<Power>();
                    for (int i = 1; i < indexedPower.Length; i++)
                    {
                        Power pw = Power.GetPower(uint.Parse(indexedPower[i]));
                        if (pw != null) toAdd.Add(pw);
                    }
                    ClassPowers[index].Add(toAdd);
                }

                _iconFileName = split[10];

                Description = uint.Parse(split[11]);

                string[] saves = split[12].Split(listDelim);
                foreach(string save in saves)
                {
                    if(!String.IsNullOrWhiteSpace(save)) ProficientSaves.Add(uint.Parse(save));
                }
            }
        }


        public uint Id;

        public string Name;

        public uint Description;

        private string _iconFileName;

        private Image _icon;

        private static Image _nullIcon;

        public Image Icon
        {
            get
            {
                if (_icon != null) return _icon;
                _icon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Classes\\" + _iconFileName + ".png");
                return _icon;
            }
        }

        public static Image NullClassImage
        {
            get
            {
                if (_nullIcon != null) return _nullIcon;
                _nullIcon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Classes\\NoClass.png");
                return _nullIcon;
            }
        }

        public int HitDie;

        public List<uint> ProficientSaves = new List<uint>();

        public int SkillChoices;

        public List<Skill> AvailableSkills = new List<Skill>();

        public int ToolChoices;

        public List<Skill> AvailableTools = new List<Skill>();

        public Dictionary<uint, List<List<Power>>> ClassPowers = new Dictionary<uint, List<List<Power>>>();

        public int SubClassLevel;

        public List<CharacterClass> SubClasses = new List<CharacterClass>();

        private List<uint> _subClassIds = new List<uint>();

        public Dictionary<uint, List<Spell>> SpellList;

        public List<int> SpellsKnown;

        public List<int> CantripsKnown;

        public Dictionary<int, List<int>> SpellSlots;

        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Classes.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    CharacterClass toAdd = new CharacterClass(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
            // Link up subclasses and spell lists.
            foreach(CharacterClass cl in _library.Values)
            {
                cl.SpellList = Mundasia.Objects.SpellList.GetSpellList(cl.Id);
                foreach(uint subClassId in cl._subClassIds)
                {
                    CharacterClass scl = CharacterClass.GetClass(subClassId);
                    if (scl != null) cl.SubClasses.Add(scl);
                }
            }
        }

        public static void LoadSpellProgression()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Spell_Progression.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while(read.Peek() >= 0)
                {
                    string spellLine = read.ReadLine();
                    string[] spellSplit = spellLine.Split(delim);
                    if(spellSplit.Length == 0)
                    {
                        continue;
                    }
                    CharacterClass chClass = GetClass(uint.Parse(spellSplit[0]));
                    if(chClass == null)
                    {
                        continue;
                    }
                    if (chClass.SpellSlots == null) chClass.SpellSlots = new Dictionary<int, List<int>>();
                    if (chClass.CantripsKnown == null) chClass.CantripsKnown = new List<int>();
                    int c = 1;
                    while(c < spellSplit.Length)
                    {
                        string[] levelSplit = spellSplit[c].Split(listDelim);
                        if(levelSplit.Length == 0)
                        {
                            continue;
                        }
                        int level = Int32.Parse(levelSplit[0]);
                        if(level == -1)
                        {
                            chClass.SpellsKnown = new List<int>();
                            int i = 1;
                            while(i < levelSplit.Length)
                            {
                                chClass.SpellsKnown.Add(Int32.Parse(levelSplit[i]));
                                i++;
                            }
                        }
                        else if(level == 0)
                        {
                            int i = 1;
                            while(i < levelSplit.Length)
                            {
                                chClass.CantripsKnown.Add(Int32.Parse(levelSplit[i]));
                                i++;
                            }
                        }
                        else
                        {
                            if (!chClass.SpellSlots.ContainsKey(level)) chClass.SpellSlots.Add(level, new List<int>());
                            int i = 1;
                            while(i < levelSplit.Length)
                            {
                                chClass.SpellSlots[level].Add(Int32.Parse(levelSplit[i]));
                                i++;
                            }
                        }
                        c++;
                    }
                }
            }
        }
        
        private static Dictionary<uint, CharacterClass> _library = new Dictionary<uint, CharacterClass>();

        public static CharacterClass GetClass(uint id)
        {
            if(_library.ContainsKey(id))
            {
                return _library[id];
            }
            return null;
        }

        public static IEnumerable<CharacterClass> GetClasses()
        {
            return _library.Values;
        }
    }
}
