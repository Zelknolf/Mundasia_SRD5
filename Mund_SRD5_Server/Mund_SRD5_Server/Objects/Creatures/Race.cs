using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                if (!int.TryParse(split[2], out Strength))
                {
                    Strength = -1;
                }
                if (!int.TryParse(split[3], out Agility))
                {
                    Agility = -1;
                }
                if (!int.TryParse(split[4], out Endurance))
                {
                    Endurance = -1;
                }
                if (!int.TryParse(split[5], out Perception))
                {
                    Perception = -1;
                }
                if (!int.TryParse(split[6], out Quickness))
                {
                    Quickness = -1;
                }
                if (!int.TryParse(split[7], out Memory))
                {
                    Memory = -1;
                }
                if (!int.TryParse(split[8], out Persuasion))
                {
                    Persuasion = -1;
                }
                if (!int.TryParse(split[9], out Glibness))
                {
                    Glibness = -1;
                }
                if (!int.TryParse(split[10], out Appearance))
                {
                    Appearance = -1;
                }
                if (!int.TryParse(split[11], out Force))
                {
                    Force = -1;
                }
                if (!int.TryParse(split[12], out Control))
                {
                    Control = -1;
                }
                if (!int.TryParse(split[13], out Discipline))
                {
                    Discipline = -1;
                }

                uint desc;
                if (uint.TryParse(split[14], out desc))
                {
                    Description = desc;
                }
                else
                {
                    Description = uint.MaxValue;
                }

                int.TryParse(split[15], out Height);
            }
            else
            {
                Id = uint.MaxValue;
                Name = "Error: " + fileLine;
                Description = uint.MaxValue;
            }
        }

        public Race(uint id, uint description, string name, int strength, int agility, int endurance, int perception, int quickness, int memory, int persuasion, int glibness, int appearance, int force, int control, int discipline)
        {
            Id = id;
            Description = description;
            Name = name;
            Strength = strength;
            Agility = agility;
            Endurance = endurance;
            Perception = perception;
            Quickness = quickness;
            Memory = memory;
            Persuasion = persuasion;
            Glibness = glibness;
            Appearance = appearance;
            Force = force;
            Control = control;
            Discipline = discipline;
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
        /// The race's base agility score
        /// </summary>
        public int Agility;

        /// <summary>
        /// The race's base endurance score
        /// </summary>
        public int Endurance;

        /// <summary>
        /// The race's base perception score
        /// </summary>
        public int Perception;

        /// <summary>
        /// The race's base quickness score
        /// </summary>
        public int Quickness;

        /// <summary>
        /// The race's base memory score
        /// </summary>
        public int Memory;

        /// <summary>
        /// The race's base persuasion score
        /// </summary>
        public int Persuasion;

        /// <summary>
        /// The race's base glibness score
        /// </summary>
        public int Glibness;

        /// <summary>
        /// The race's base appearance score
        /// </summary>
        public int Appearance;

        /// <summary>
        /// The race's base force score
        /// </summary>
        public int Force;

        /// <summary>
        /// The race's base control score
        /// </summary>
        public int Control;

        /// <summary>
        /// The race's base discipline score
        /// </summary>
        public int Discipline;

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
    }
}
