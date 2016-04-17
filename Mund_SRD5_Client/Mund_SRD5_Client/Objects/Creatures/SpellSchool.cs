using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Mundasia.Objects
{
    public class SpellSchool
    {
        /// <summary>
        /// Default delimiter used to split properties from config files.
        /// </summary>
        const char delim = '|';

        /// <summary>
        /// Turns a line on the powers resource files into a power object.
        /// </summary>
        /// <param name="fileLine">the string from a line in the config file</param>
        public SpellSchool(string fileLine)
        {
            string[] split = fileLine.Split(new char[] { delim });
            uint index = 0;
            if(uint.TryParse(split[0], out index))
            {
                Id = index;

                Name = split[1];
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
        public SpellSchool(uint id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// The school's Id
        /// </summary>
        public uint Id;

        /// <summary>
        /// The school's name
        /// </summary>
        public string Name;

        /// <summary>
        /// Caching for loaded schools.
        /// </summary>
        private static Dictionary<uint, SpellSchool> _library = new Dictionary<uint, SpellSchool>();

        /// <summary>
        /// Load schools from the DataArrays\Spell_Schools.txt folder and store them in _library
        /// </summary>
        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Spell_Schools.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    SpellSchool toAdd = new SpellSchool(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        /// <summary>
        /// Fetch a school from _library
        /// </summary>
        /// <param name="skillId">The school's Id</param>
        /// <returns>The skill on success; null on failure</returns>
        public static SpellSchool GetSpellSchool(uint powerId)
        {
            if (_library.ContainsKey(powerId))
            {
                return _library[powerId];
            }
            return null;
        }

        /// <summary>
        /// Get all currently-loaded spell schools as an enumerable.
        /// </summary>
        /// <returns>an IEnumerable containing all loaded spell schools</returns>
        public static IEnumerable<SpellSchool> GetSpellSchools()
        {
            return _library.Values;
        }
    }
}
