using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Mundasia.Objects
{
    public class SpellList
    {
        /// <summary>
        /// Default delimiter used to split properties from config files.
        /// </summary>
        const char delim = '|';

        /// <summary>
        /// Default delimiter used to split numbers apart.
        /// </summary>
        const char listDelim = '[';

        public static void ProcessSpellList(string fileLine)
        {
            string[] split = fileLine.Split(new char[] { delim });
            uint index = 0;
            if (split.Length > 1 &&
                uint.TryParse(split[0], out index))
            {
                if(!_library.ContainsKey(index))
                {
                    _library.Add(index, new Dictionary<uint, List<Spell>>());
                }

                for(int i = 1; i < split.Length; i++)
                {
                    string[] levelSplit = split[i].Split(listDelim);
                    uint level = 0;
                    if(levelSplit.Length > 1 &&
                       uint.TryParse(levelSplit[0], out level))
                    {
                        if(!_library[index].ContainsKey(level))
                        {
                            _library[index].Add(level, new List<Spell>());
                        }
                        for(int s = 1; s < levelSplit.Length; s++)
                        {
                            uint spellIndex = 0;
                            if(uint.TryParse(levelSplit[s], out spellIndex))
                            {
                                Spell toAddSpell = Spell.GetSpell(spellIndex);
                                if(toAddSpell != null &&
                                   !_library[index][level].Contains(toAddSpell))
                                {
                                    _library[index][level].Add(toAddSpell);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void AddSpellToList(uint classId, uint spellLevel, Spell spell)
        {
            if(!_library.ContainsKey(classId))
            {
                _library.Add(classId, new Dictionary<uint, List<Spell>>());
            }
            if(!_library[classId].ContainsKey(spellLevel))
            {
                _library[classId].Add(spellLevel, new List<Spell>());
            }
            if(!_library[classId][spellLevel].Contains(spell))
            {
                _library[classId][spellLevel].Add(spell);
            }
        }

        private static Dictionary<uint, Dictionary<uint, List<Spell>>> _library = new Dictionary<uint, Dictionary<uint, List<Spell>>>();

        /// <summary>
        /// Load the spell lists and cache them for future use.
        /// </summary>
        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Spell_Lists.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    ProcessSpellList(read.ReadLine());
                }
            }
        }

        /// <summary>
        /// Fetch a school from _library
        /// </summary>
        /// <param name="skillId">The school's Id</param>
        /// <returns>The skill on success; null on failure</returns>
        public static Dictionary<uint, List<Spell>> GetSpellList(uint classId)
        {
            if (_library.ContainsKey(classId))
            {
                return _library[classId];
            }
            return null;
        }
    }
}
