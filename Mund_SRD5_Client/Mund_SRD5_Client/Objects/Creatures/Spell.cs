using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Mundasia.Objects
{
    public class Spell
    {
        /// <summary>
        /// Default delimiter used to split properties from config files.
        /// </summary>
        const char delim = '|';

        /// <summary>
        /// Turns a line on the powers resource files into a power object.
        /// </summary>
        /// <param name="fileLine">the string from a line in the config file</param>
        public Spell(string fileLine)
        {
            string[] split = fileLine.Split(new char[] { delim });
            uint index = 0;
            if(uint.TryParse(split[0], out index))
            {
                Id = index;

                Name = split[1];

                uint _school = 0;
                if(uint.TryParse(split[2], out _school))
                {
                    School = SpellSchool.GetSpellSchool(_school);
                }

                int.TryParse(split[3], out Level);

                int ritual = 0;
                if (int.TryParse(split[4], out ritual))
                {
                    if (ritual > 0) Ritual = true;
                }
            }
            else
            {
                Id = uint.MaxValue;
                Name = "Error: " + fileLine;
            }
        }

        /// <summary>
        /// Create a new power with given properties
        /// </summary>
        /// <param name="id">the ID of the power</param>
        /// <param name="name">the Name of the power</param>
        /// <param name="description">The string reference for the power's description</param>
        public Spell(uint id, string name, uint description)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// The spell's Id
        /// </summary>
        public uint Id;

        /// <summary>
        /// The spell's name
        /// </summary>
        public string Name;

        /// <summary>
        /// The spell's school
        /// </summary>
        public SpellSchool School;

        /// <summary>
        /// The spell's level
        /// </summary>
        public int Level;

        /// <summary>
        /// Whether or not the spell may be cast as a ritual.
        /// </summary>
        public bool Ritual;

        /// <summary>
        /// Caching for loaded powers.
        /// </summary>
        private static Dictionary<uint, Spell> _library = new Dictionary<uint, Spell>();

        /// <summary>
        /// Load powers from the DataArrays\Spell.txt folder and store them in _library
        /// </summary>
        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Spell.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    Spell toAdd = new Spell(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        /// <summary>
        /// Fetch a Power from _library
        /// </summary>
        /// <param name="skillId">The power's Id</param>
        /// <returns>The power on success; null on failure</returns>
        public static Spell GetSpell(uint powerId)
        {
            if (_library.ContainsKey(powerId))
            {
                return _library[powerId];
            }
            return null;
        }

        /// <summary>
        /// Get all currently-loaded powers as an enumerable.
        /// </summary>
        /// <returns>an IEnumerable containing all loaded powers</returns>
        public static IEnumerable<Spell> GetSpells()
        {
            return _library.Values;
        }
    }
}
