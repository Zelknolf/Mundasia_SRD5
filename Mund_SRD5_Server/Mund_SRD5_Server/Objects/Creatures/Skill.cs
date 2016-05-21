using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.IO;

namespace Mundasia.Objects
{
    public class Skill
    {
        /// <summary>
        /// Turns a line in the skills resource file into a skill to be used in play.
        /// </summary>
        /// <param name="fileLine">the line from the resource</param>
        public Skill(string fileLine)
        {
            string[] split = fileLine.Split(new char[] { '|' });
            uint index = 0;
            if (uint.TryParse(split[0], out index) &&
                !_library.ContainsKey(index))
            {
                Id = index;

                Ability = uint.Parse(split[1]);

                Name = split[2];

                uint desc;
                if (uint.TryParse(split[3], out desc))
                {
                    Description = desc;
                }
                else
                {
                    Description = uint.MaxValue;
                }
                _iconFileName = split[4];
            }
            else
            {
                Id = uint.MaxValue;
                Name = "Error: "+fileLine;
                Description = uint.MaxValue;
                _icon = NullIcon;
            }
        }

        /// <summary>
        /// Produces a new skill with a definition of its properties
        /// </summary>
        /// <param name="id">the skill's id</param>
        /// <param name="nam">the skill's name</param>
        /// <param name="cat">the skill's category</param>
        /// <param name="description">the uint reference to the skill's description</param>
        public Skill(uint id, uint ability, string nam, uint description) 
        {
            Id = id;
            Ability = ability;
            Name = nam;
            Description = description;
        }

        /// <summary>
        /// The skill's Id
        /// </summary>
        public uint Id;

        /// <summary>
        /// The skill's name
        /// </summary>
        public string Name;

        /// <summary>
        /// The uint which references the string to be used. This is not translated
        /// to an actual string here as the server does not handle client-side strings.
        /// </summary>
        public uint Description;

        /// <summary>
        /// An unsigned int referring to the ability score that the skill uses.
        /// </summary>
        public uint Ability;

        private string _iconFileName;

        private Image _icon;

        private static Image _nullIcon;

        public Image Icon
        {
            get
            {
                if (_icon != null) return _icon;
                _icon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Skills\\" + _iconFileName + ".png");
                return _icon;
            }
        }
        
        public static Image NullIcon
        {
            get
            {
                if (_nullIcon != null) return _nullIcon;
                _nullIcon = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Skills\\NoSkill.png");
                return _nullIcon;
            }
        }

        /// <summary>
        /// Static storage for cached skills.
        /// </summary>
        private static Dictionary<uint, Skill> _library = new Dictionary<uint, Skill>();

        /// <summary>
        /// Load skills from the DataArrays\Skills.txt folder and store them in _library
        /// </summary>
        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Skills.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    Skill toAdd = new Skill(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        /// <summary>
        /// Fetch a Skill from _library
        /// </summary>
        /// <param name="skillId">The skill's Id</param>
        /// <returns>The skill on success; null on failure</returns>
        public static Skill GetSkill(uint skillId)
        {
            if (_library.ContainsKey(skillId))
            {
                return _library[skillId];
            }
            return null;
        }

        public static IEnumerable<Skill> GetSkills()
        {
            return _library.Values;
        }
    }
}