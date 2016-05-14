using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.IO;

namespace Mundasia.Objects
{
    public class Race
    {
        public Race(string fileLine)
        {
            string[] split = fileLine.Split(new char[] { '|' });
            uint index = 0;
            if (uint.TryParse(split[0], out index) &&
                !_library.ContainsKey(index))
            {
                Id = index;

                Name = split[1];

                if (!int.TryParse(split[2], out Height))
                {
                    Height = 4;
                }
                if (!int.TryParse(split[3], out Movement))
                {
                    Movement = 6;
                }
                if (!int.TryParse(split[4], out Strength))
                {
                    Strength = 1;
                }
                if (!int.TryParse(split[5], out Dexterity))
                {
                    Dexterity = 1;
                }
                if (!int.TryParse(split[6], out Constitution))
                {
                    Constitution = 1;
                }
                if (!int.TryParse(split[7], out Intelligence))
                {
                    Intelligence = 1;
                }
                if (!int.TryParse(split[8], out Wisdom))
                {
                    Wisdom = 1;
                }
                if (!int.TryParse(split[9], out Charisma))
                {
                    Charisma = 1;
                }
                if (!uint.TryParse(split[10], out Description))
                {
                    Description = 13000;
                }
                if (!int.TryParse(split[11], out FreeSkills))
                {
                    FreeSkills = 0;
                }

                AutomaticSkills = new List<int>();
                string[] autoSkills = split[12].Split('[');
                foreach(string autoSkill in autoSkills)
                {
                    int sk;
                    if (Int32.TryParse(autoSkill, out sk)) AutomaticSkills.Add(sk);
                }

                SelectedSkill = new List<int>();
                string[] selSkills = split[13].Split('[');
                foreach(string selSkill in selSkills)
                {
                    int sk;
                    if (Int32.TryParse(selSkill, out sk)) SelectedSkill.Add(sk);
                }

                Powers = new List<int>();
                string[] pows = split[14].Split('[');
                foreach(string pow in pows)
                {
                    int sk;
                    if (Int32.TryParse(pow, out sk)) Powers.Add(sk);
                }

                _iconFileName = split[15];
            }
            else
            {
                Id = uint.MaxValue;
                Name = "Error: " + fileLine;
                Description = uint.MaxValue;
            }
        }

        public Race(uint id, uint description, string name, int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma, int height, int movement, int freeSkills, List<int> automaticSkills, List<int> freeSkillOptions, List<int> powers)
        {
            Id = id;
            Description = description;
            Name = name;
            Strength = strength;
            Dexterity = dexterity;
            Constitution = constitution;
            Intelligence = intelligence;
            Wisdom = wisdom;
            Charisma = charisma;
            Height = height;
            Movement = movement;
            FreeSkills = freeSkills;
            AutomaticSkills = automaticSkills;
            SelectedSkill = freeSkillOptions;
            Powers = powers;
        }

        /// <summary>
        /// Load skills from the DataArrays\Races.txt folder and store them in _library
        /// </summary>
        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Races.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    Race toAdd = new Race(read.ReadLine());
                    toAdd.PlayableHairStyles = new Dictionary<int, int>();
                    foreach(string hair in Directory.GetFiles(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Characters\\"+ toAdd.Id + "\\0\\Hair"))
                    {
                        string s = hair.Substring(hair.Length - 11, 2);
                        s = s.Trim('_');
                        int num;
                        int.TryParse(s, out num);
                        if(!toAdd.PlayableHairStyles.ContainsKey(0))
                        {
                            toAdd.PlayableHairStyles.Add(0, num);
                        }
                        else if(toAdd.PlayableHairStyles[0] + 1 == num)
                        {
                            toAdd.PlayableHairStyles[0]++;
                        }
                    }
                    foreach (string hair in Directory.GetFiles(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Characters\\" + toAdd.Id + "\\1\\Hair"))
                    {
                        string s = hair.Substring(hair.Length - 11, 2);
                        s = s.Trim('_');
                        int num;
                        int.TryParse(s, out num);
                        if (!toAdd.PlayableHairStyles.ContainsKey(1))
                        {
                            toAdd.PlayableHairStyles.Add(1, num);
                        }
                        else if (toAdd.PlayableHairStyles[1] + 1 == num)
                        {
                            toAdd.PlayableHairStyles[1]++;
                        }
                    }
                    _library.Add(toAdd.Id, toAdd);
                }
            }

            file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Race_SkinTone.txt";
            strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    string[] raceLine = read.ReadLine().Split(new char[] { '|' });
                    uint race = 0;
                    uint.TryParse(raceLine[0], out race);
                    List<SkinColor> colors = new List<SkinColor>();
                    foreach(string colorLine in raceLine)
                    {
                        if (!colorLine.Contains(',')) continue;
                        colors.Add(new SkinColor(colorLine));
                    }
                    _library[race].SkinColors = colors;   
                }
            }

            file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Race_HairColor.txt";
            strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    string[] raceLine = read.ReadLine().Split(new char[] { '|' });
                    uint race = 0;
                    uint.TryParse(raceLine[0], out race);
                    List<SkinColor> colors = new List<SkinColor>();
                    foreach (string colorLine in raceLine)
                    {
                        if (!colorLine.Contains(',')) continue;
                        colors.Add(new SkinColor(colorLine));
                    }
                    _library[race].HairColors = colors;
                }
            }
        }

        public static Race GetRace(uint index)
        {
            if(_library.ContainsKey(index))
            {
                return _library[index];
            }
            return null;
        }

        public static IEnumerable<Race> GetRaces()
        {
            return _library.Values;
        }

        /// <summary>
        /// Storage of loaded races, after Load has been called.
        /// </summary>
        private static Dictionary<uint, Race> _library = new Dictionary<uint, Race>();

        /// <summary>
        /// The unique Id of the race
        /// </summary>
        public uint Id;

        /// <summary>
        /// The Id of the race's description
        /// </summary>
        public uint Description;

        /// <summary>
        /// The race's name
        /// </summary>
        public string Name;

        /// <summary>
        /// The race's base strength score
        /// </summary>
        public int Strength;

        /// <summary>
        /// The race's base dexterity score
        /// </summary>
        public int Dexterity;

        /// <summary>
        /// The race's base constitution score
        /// </summary>
        public int Constitution;

        /// <summary>
        /// The race's base intelligence score
        /// </summary>
        public int Intelligence;

        /// <summary>
        /// The race's base wisdom score
        /// </summary>
        public int Wisdom;

        /// <summary>
        /// The race's base charisma score
        /// </summary>
        public int Charisma;

        /// <summary>
        /// The race's base movement in tiles.
        /// </summary>
        public int Movement;

        /// <summary>
        /// The number of free skill selections that members of the race can take.
        /// </summary>
        public int FreeSkills;

        /// <summary>
        /// Skills that are automatically granted to members of this race.
        /// </summary>
        public List<int> AutomaticSkills;

        /// <summary>
        /// A list of skills that members of this race may pick the free skill from.
        /// </summary>
        public List<int> SelectedSkill;

        /// <summary>
        /// A list of powers that members of this race receive.
        /// </summary>
        public List<int> Powers;

        /// <summary>
        /// The race's available skin colors.
        /// </summary>
        public List<SkinColor> SkinColors;

        /// <summary>
        /// The race's available hair colors.
        /// </summary>
        public List<SkinColor> HairColors;

        /// <summary>
        /// The race's height, in tiles.
        /// </summary>
        public int Height;

        /// <summary>
        /// The hair styles available to PCs. Index is gender.
        /// </summary>
        public Dictionary<int, int> PlayableHairStyles;

        private string _iconFileName;

        public Image _icon;

        private static Image _nullIcon;

        public Image Icon
        {
            get
            {
                if (_icon != null) return _icon;
                _icon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Races\\" + _iconFileName + ".png");
                return _icon;
            }
        }

        public static Image NullRaceImage
        {
            get
            {
                if (_nullIcon != null) return _nullIcon;
                _nullIcon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Classes\\NoClass.png");
                return _nullIcon;
            }
        }
    }
}
